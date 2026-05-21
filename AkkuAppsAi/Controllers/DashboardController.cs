using AkkuAppsAi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AkkuAppsAi.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly AppDbContext _db;

    public DashboardController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue("UserId");
        var userName = User.FindFirstValue(ClaimTypes.Name);
        var userRole = User.FindFirstValue("UserRole");
        var userType = User.FindFirstValue("UserType");

        var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId);

        ViewBag.UserId = userId;
        ViewBag.UserName = userName;
        ViewBag.UserRole = userRole;
        ViewBag.UserType = userType;
        ViewBag.IsAdmin = userRole == "admin";
        ViewBag.UserCoins = user?.CoinBalance ?? 0;

        return View();
    }

    [Authorize(Roles = "admin")]
    public IActionResult Admin()
    {
        return RedirectToAction("Dashboard", "Admin");
    }
}
