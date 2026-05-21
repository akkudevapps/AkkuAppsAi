using AkkuAppsAi.Data;
using AkkuAppsAi.Models;
using AkkuAppsAi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AkkuAppsAi.Controllers;

[Authorize(Roles = "admin")]
public class AdminController : Controller
{
    private readonly AppDbContext _db;
    private readonly AnalyticsService _analytics;
    private readonly CoinService _coins;
    private readonly MarketplaceService _marketplace;

    public AdminController(AppDbContext db, AnalyticsService analytics, CoinService coins, MarketplaceService marketplace)
    {
        _db = db;
        _analytics = analytics;
        _coins = coins;
        _marketplace = marketplace;
    }

    public IActionResult Index()
    {
        return RedirectToAction("Dashboard");
    }

    public async Task<IActionResult> Dashboard()
    {
        ViewBag.TotalUsers = await _db.Users.CountAsync();
        ViewBag.TotalGoods = await _db.DigitalGoods.CountAsync();
        ViewBag.TotalPurchases = await _db.Purchases.CountAsync();
        ViewBag.TotalVisitors = await _analytics.GetTotalVisitorsAsync();
        ViewBag.TodayVisitors = await _analytics.GetVisitorsTodayAsync();
        ViewBag.PendingOrders = await _db.CoinPurchaseOrders.CountAsync(o => o.Status == "Pending");
        ViewBag.RecentTransactions = await _db.CoinTransactions
            .OrderByDescending(t => t.CreatedAt).Take(10).ToListAsync();
        return View();
    }

    public async Task<IActionResult> Users(string? search)
    {
        var query = _db.Users.AsQueryable();
        if (!string.IsNullOrEmpty(search))
            query = query.Where(u => u.Name.Contains(search) || u.Email.Contains(search));
        var users = await query.OrderByDescending(u => u.CreatedAt).ToListAsync();
        return View(users);
    }

    public async Task<IActionResult> UserDetail(string id)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == id);
        if (user == null) return NotFound();
        ViewBag.Transactions = await _db.CoinTransactions
            .Where(t => t.UserId == id).OrderByDescending(t => t.CreatedAt).ToListAsync();
        ViewBag.Purchases = await _db.Purchases
            .Where(p => p.UserId == id).OrderByDescending(p => p.CreatedAt).ToListAsync();
        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> ToggleBan(string userId, bool isBanned)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        if (user == null) return NotFound();
        user.IsBanned = isBanned;
        user.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return RedirectToAction("Users");
    }

    public async Task<IActionResult> Coins()
    {
        var users = await _db.Users.OrderByDescending(u => u.CoinBalance).ToListAsync();
        ViewBag.Transactions = await _db.CoinTransactions
            .OrderByDescending(t => t.CreatedAt).Take(100).ToListAsync();
        return View(users);
    }

    [HttpPost]
    public async Task<IActionResult> AdjustCoins(string userId, decimal amount, string description)
    {
        var adminId = User.FindFirstValue("UserId");
        if (amount > 0)
            await _coins.AddCoinsAsync(userId, amount, "admin_adjust", description ?? "Admin adjustment");
        else
            await _coins.DeductCoinsAsync(userId, Math.Abs(amount), "admin_adjust", description ?? "Admin adjustment");
        return RedirectToAction("Coins");
    }

    [HttpPost]
    public async Task<IActionResult> PromoteUser(string userId, string planName, decimal coinsCharged)
    {
        var adminId = User.FindFirstValue("UserId");
        var result = await _coins.DeductCoinsAsync(userId, coinsCharged, "promotion", $"Promoted via {planName}");
        if (!result.Success) return BadRequest(result.Message);

        var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        if (user == null) return NotFound();

        user.UserType = "AkkuProUser";
        user.Role = "pro";
        user.UpdatedAt = DateTime.UtcNow;

        _db.UserPromotions.Add(new UserPromotion
        {
            PromotionId = Guid.NewGuid().ToString(),
            UserId = userId,
            NewUserType = "AkkuProUser",
            PlanName = planName,
            CoinsCharged = coinsCharged,
            ApprovedBy = adminId,
            CreatedAt = DateTime.UtcNow
        });

        await _db.SaveChangesAsync();
        return RedirectToAction("Users");
    }

    public async Task<IActionResult> PurchaseOrders()
    {
        var orders = await _db.CoinPurchaseOrders
            .OrderByDescending(o => o.CreatedAt).ToListAsync();
        return View(orders);
    }

    [HttpPost]
    public async Task<IActionResult> ApproveOrder(string orderId)
    {
        var order = await _db.CoinPurchaseOrders.FirstOrDefaultAsync(o => o.OrderId == orderId);
        if (order == null || order.Status != "Pending") return NotFound();

        order.Status = "Approved";
        order.ApprovedBy = User.FindFirstValue("UserId");
        order.ApprovedAt = DateTime.UtcNow;

        await _coins.AddCoinsAsync(order.UserId, order.Coins, "upi_purchase", $"UPI purchase of {order.Coins} coins");
        return RedirectToAction("PurchaseOrders");
    }

    [HttpPost]
    public async Task<IActionResult> RejectOrder(string orderId)
    {
        var order = await _db.CoinPurchaseOrders.FirstOrDefaultAsync(o => o.OrderId == orderId);
        if (order == null || order.Status != "Pending") return NotFound();
        order.Status = "Rejected";
        order.ApprovedBy = User.FindFirstValue("UserId");
        order.ApprovedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return RedirectToAction("PurchaseOrders");
    }

    public async Task<IActionResult> DigitalGoods()
    {
        var goods = await _db.DigitalGoods.OrderByDescending(g => g.CreatedAt).ToListAsync();
        return View(goods);
    }

    [HttpPost]
    public async Task<IActionResult> CreateDigitalGood(string title, string? description, decimal priceInCoins,
        bool hasCommission, string? category, string? thumbnailUrl, string? fileUrl)
    {
        var userId = User.FindFirstValue("UserId");
        await _marketplace.CreateGoodAsync(title, description, priceInCoins, hasCommission,
            category, thumbnailUrl, fileUrl, userId!);
        return RedirectToAction("DigitalGoods");
    }

    [HttpPost]
    public async Task<IActionResult> ToggleGood(string goodId, bool isActive)
    {
        var good = await _db.DigitalGoods.FirstOrDefaultAsync(g => g.GoodId == goodId);
        if (good == null) return NotFound();
        good.IsActive = isActive;
        await _db.SaveChangesAsync();
        return RedirectToAction("DigitalGoods");
    }

    public async Task<IActionResult> SiteAnalytics()
    {
        ViewBag.TotalVisitors = await _analytics.GetTotalVisitorsAsync();
        ViewBag.TodayVisitors = await _analytics.GetVisitorsTodayAsync();

        var byDate = await _analytics.GetVisitorsByDateAsync(30);
        ViewBag.VisitorsByDate = byDate.Select(d => new { d.Date, d.Count }).ToList<object>();

        var byCountry = await _analytics.GetVisitorsByCountryAsync();
        ViewBag.VisitorsByCountry = byCountry.Select(c => new { c.Country, c.Count }).ToList<object>();

        var byTz = await _analytics.GetVisitorsByTimeZoneAsync();
        ViewBag.VisitorsByTimeZone = byTz.Select(t => new { t.TimeZone, t.Count }).ToList<object>();

        return View();
    }

    public async Task<IActionResult> Promotions()
    {
        var promotions = await _db.UserPromotions
            .OrderByDescending(p => p.CreatedAt).ToListAsync();
        return View(promotions);
    }

    public async Task<IActionResult> Logs()
    {
        var transactions = await _db.CoinTransactions
            .OrderByDescending(t => t.CreatedAt).Take(200).ToListAsync();
        return View(transactions);
    }

    public IActionResult Settings()
    {
        return View();
    }
}
