using Microsoft.EntityFrameworkCore;
using AkkuAppsAi.Data;
using AkkuAppsAi.Models;
using AkkuAppsAi.Helpers;
using AkkuAppsAi.Services;
using AkkuAppsAi.Middleware;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<AnalyticsService>();
builder.Services.AddScoped<CoinService>();
builder.Services.AddScoped<MarketplaceService>();
builder.Services.AddScoped<IOllamaService, OllamaService>();
builder.Services.AddHttpClient();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.SlidingExpiration = true;
})
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Google:ClientId"]!;
    options.ClientSecret = builder.Configuration["Google:ClientSecret"]!;
    options.CallbackPath = "/signin-google";
    options.SaveTokens = true;

    options.Events = new Microsoft.AspNetCore.Authentication.OAuth.OAuthEvents
    {
        OnCreatingTicket = async context =>
        {
            var email = context.Principal?.FindFirstValue(ClaimTypes.Email);
            var name = context.Principal?.FindFirstValue(ClaimTypes.Name);
            var googleId = context.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
            var avatar = context.Principal?.FindFirstValue("picture");

            if (string.IsNullOrEmpty(email)) return;

            var db = context.HttpContext.RequestServices.GetRequiredService<AppDbContext>();
            var emailHash = CryptoHelper.HashEmail(email);

            // Check by email OR google_id
            var user = await db.Users
                .FirstOrDefaultAsync(u => u.Email == email || u.GoogleId == googleId);

            if (user != null)
            {
                if (user.IsBanned)
                {
                    context.Fail("Account suspended");
                    return;
                }
                user.LastLogin = DateTime.UtcNow;
                user.GoogleId = googleId;
                user.Name = name ?? user.Name;
                user.Avatar = avatar ?? user.Avatar;
                user.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                var userId = Guid.NewGuid().ToString();
                var isAdmin = email == "leoinfotech.chinnamanur@gmail.com";
                user = new User
                {
                    UserId = userId,
                    GoogleId = googleId,
                    Email = email,
                    EmailHash = emailHash,
                    Name = name ?? "User",
                    Avatar = avatar,
                    UserType = isAdmin ? "AkkuAdmin" : "AkkuUser",
                    Role = isAdmin ? "admin" : "user",
                    CoinBalance = 100.0000m,
                    WalletBalance = 0.00m,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                db.Users.Add(user);
                db.CoinTransactions.Add(new CoinTransaction
                {
                    TxnId = Guid.NewGuid().ToString(),
                    UserId = userId,
                    ReferenceType = "new_user",
                    Amount = 100.0000m,
                    BalanceAfter = 100.0000m,
                    Description = "Welcome bonus",
                    CreatedAt = DateTime.UtcNow
                });
            }

            await db.SaveChangesAsync();

            var identity = context.Principal?.Identity as ClaimsIdentity;
            identity?.AddClaim(new Claim("UserId", user.UserId));
            identity?.AddClaim(new Claim("UserRole", user.Role));
            identity?.AddClaim(new Claim("UserType", user.UserType));
            identity?.AddClaim(new Claim(ClaimTypes.Role, user.Role));
        }
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseVisitTracking();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (!db.CoinPackages.Any())
    {
        db.CoinPackages.AddRange(
            new CoinPackage { Coins = 100, PriceINR = 50, Description = "Starter Pack", IsActive = true },
            new CoinPackage { Coins = 500, PriceINR = 200, Description = "Popular Pack", IsActive = true },
            new CoinPackage { Coins = 1000, PriceINR = 350, Description = "Pro Pack", IsActive = true },
            new CoinPackage { Coins = 5000, PriceINR = 1500, Description = "Ultra Pack", IsActive = true }
        );
        db.SaveChanges();
    }
}

app.Run();
