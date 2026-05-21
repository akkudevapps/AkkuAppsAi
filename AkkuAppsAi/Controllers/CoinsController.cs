using AkkuAppsAi.Data;
using AkkuAppsAi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AkkuAppsAi.Controllers;

[Authorize]
public class CoinsController : Controller
{
    private readonly AppDbContext _db;

    public CoinsController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Buy()
    {
        var packages = await _db.CoinPackages.Where(p => p.IsActive).ToListAsync();
        return View(packages);
    }

    [HttpPost]
    public async Task<IActionResult> PlaceOrder(int packageId)
    {
        var package = await _db.CoinPackages.FirstOrDefaultAsync(p => p.PackageId == packageId && p.IsActive);
        if (package == null) return NotFound();

        var userId = User.FindFirstValue("UserId");

        var order = new CoinPurchaseOrder
        {
            OrderId = Guid.NewGuid().ToString(),
            UserId = userId!,
            PackageId = packageId,
            Coins = package.Coins,
            AmountINR = package.PriceINR,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };

        _db.CoinPurchaseOrders.Add(order);
        await _db.SaveChangesAsync();

        return RedirectToAction("OrderConfirmation", new { orderId = order.OrderId });
    }

    public async Task<IActionResult> OrderConfirmation(string orderId)
    {
        var order = await _db.CoinPurchaseOrders.FirstOrDefaultAsync(o => o.OrderId == orderId);
        if (order == null) return NotFound();
        return View(order);
    }

    public async Task<IActionResult> MyOrders()
    {
        var userId = User.FindFirstValue("UserId");
        var orders = await _db.CoinPurchaseOrders
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
        return View(orders);
    }

    [HttpPost]
    public async Task<IActionResult> SubmitTransactionId(string orderId, string upiTransactionId)
    {
        var userId = User.FindFirstValue("UserId");
        var order = await _db.CoinPurchaseOrders.FirstOrDefaultAsync(o =>
            o.OrderId == orderId && o.UserId == userId && o.Status == "Pending");
        if (order == null) return NotFound();

        order.UpiTransactionId = upiTransactionId;
        await _db.SaveChangesAsync();

        TempData["Success"] = "Transaction ID submitted! Awaiting admin approval.";
        return RedirectToAction("MyOrders");
    }
}
