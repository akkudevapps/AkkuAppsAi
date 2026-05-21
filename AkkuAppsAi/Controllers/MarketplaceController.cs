using AkkuAppsAi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AkkuAppsAi.Controllers;

public class MarketplaceController : Controller
{
    private readonly MarketplaceService _marketplace;
    private readonly CoinService _coins;

    public MarketplaceController(MarketplaceService marketplace, CoinService coins)
    {
        _marketplace = marketplace;
        _coins = coins;
    }

    public async Task<IActionResult> Index(string? category)
    {
        ViewBag.Categories = await _marketplace.GetCategoriesAsync();
        ViewBag.SelectedCategory = category;
        var goods = await _marketplace.GetActiveGoodsAsync(category);
        return View(goods);
    }

    public async Task<IActionResult> Details(string id)
    {
        var good = await _marketplace.GetGoodAsync(id);
        if (good == null) return NotFound();
        return View(good);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Buy(string goodId)
    {
        var userId = User.FindFirstValue("UserId");
        var result = await _coins.BuyDigitalGoodAsync(userId!, goodId);
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction("Details", new { id = goodId });
        }
        TempData["Success"] = result.Message;
        return RedirectToAction("Index");
    }

    [Authorize(Roles = "admin,pro")]
    public IActionResult Create()
    {
        return View();
    }

    [Authorize(Roles = "admin,pro")]
    [HttpPost]
    public async Task<IActionResult> Create(string title, string? description, decimal priceInCoins,
        bool hasCommission, string? category, string? thumbnailUrl, string? fileUrl)
    {
        var userId = User.FindFirstValue("UserId");
        await _marketplace.CreateGoodAsync(title, description, priceInCoins, hasCommission,
            category, thumbnailUrl, fileUrl, userId!);
        TempData["Success"] = "Digital good created!";
        return RedirectToAction("Index");
    }

    [Authorize]
    public async Task<IActionResult> MyPurchases()
    {
        var userId = User.FindFirstValue("UserId");
        var purchases = await _marketplace.GetUserPurchasesAsync(userId!);
        return View(purchases);
    }

    [Authorize(Roles = "admin,pro")]
    public async Task<IActionResult> MyGoods()
    {
        var userId = User.FindFirstValue("UserId");
        var goods = await _marketplace.GetUserGoodsAsync(userId!);
        return View(goods);
    }
}
