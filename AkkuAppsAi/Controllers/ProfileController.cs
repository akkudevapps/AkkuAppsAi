using AkkuAppsAi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AkkuAppsAi.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly AppDbContext _db;

    public ProfileController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue("UserId");
        var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        if (user == null) return NotFound();

        ViewBag.Transactions = await _db.CoinTransactions
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.CreatedAt).Take(20).ToListAsync();

        ViewBag.Purchases = await _db.Purchases
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.CreatedAt).Take(10).ToListAsync();

        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateBio(string bio)
    {
        var userId = User.FindFirstValue("UserId");
        var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        if (user == null) return NotFound();
        user.Bio = bio;
        user.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}
