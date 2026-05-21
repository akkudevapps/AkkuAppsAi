using AkkuAppsAi.Data;
using AkkuAppsAi.Models;
using Microsoft.EntityFrameworkCore;

namespace AkkuAppsAi.Services;

public class MarketplaceService
{
    private readonly AppDbContext _db;

    public MarketplaceService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<DigitalGood>> GetActiveGoodsAsync(string? category = null)
    {
        var query = _db.DigitalGoods.Where(g => g.IsActive);
        if (!string.IsNullOrEmpty(category))
            query = query.Where(g => g.Category == category);
        return await query.OrderByDescending(g => g.CreatedAt).ToListAsync();
    }

    public async Task<List<string>> GetCategoriesAsync()
    {
        return await _db.DigitalGoods
            .Where(g => g.IsActive && g.Category != null)
            .Select(g => g.Category!)
            .Distinct()
            .ToListAsync();
    }

    public async Task<DigitalGood?> GetGoodAsync(string goodId)
    {
        return await _db.DigitalGoods.FirstOrDefaultAsync(g => g.GoodId == goodId);
    }

    public async Task<List<DigitalGood>> GetUserGoodsAsync(string userId)
    {
        return await _db.DigitalGoods
            .Where(g => g.CreatedByUserId == userId)
            .OrderByDescending(g => g.CreatedAt)
            .ToListAsync();
    }

    public async Task<DigitalGood> CreateGoodAsync(string title, string? description, decimal priceInCoins,
        bool hasCommission, string? category, string? thumbnailUrl, string? fileUrl, string createdByUserId)
    {
        var good = new DigitalGood
        {
            GoodId = Guid.NewGuid().ToString(),
            Title = title,
            Description = description,
            PriceInCoins = priceInCoins,
            HasCommission = hasCommission,
            Category = category,
            ThumbnailUrl = thumbnailUrl,
            FileUrl = fileUrl,
            CreatedByUserId = createdByUserId,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };
        _db.DigitalGoods.Add(good);
        await _db.SaveChangesAsync();
        return good;
    }

    public async Task<bool> UpdateGoodAsync(string goodId, string title, string? description, decimal priceInCoins,
        bool hasCommission, string? category, string? thumbnailUrl, string? fileUrl, bool isActive)
    {
        var good = await _db.DigitalGoods.FirstOrDefaultAsync(g => g.GoodId == goodId);
        if (good == null) return false;

        good.Title = title;
        good.Description = description;
        good.PriceInCoins = priceInCoins;
        good.HasCommission = hasCommission;
        good.Category = category;
        good.ThumbnailUrl = thumbnailUrl;
        good.FileUrl = fileUrl;
        good.IsActive = isActive;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<List<Purchase>> GetUserPurchasesAsync(string userId)
    {
        return await _db.Purchases
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }
}
