using AkkuAppsAi.Data;
using AkkuAppsAi.Models;
using Microsoft.EntityFrameworkCore;

namespace AkkuAppsAi.Services;

public class CoinService
{
    private readonly AppDbContext _db;

    public CoinService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<User?> GetUserAsync(string userId)
    {
        return await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId);
    }

    public async Task<(bool Success, string Message)> DeductCoinsAsync(string userId, decimal amount, string referenceType, string? description)
    {
        var user = await GetUserAsync(userId);
        if (user == null) return (false, "User not found");
        if (user.CoinBalance < amount) return (false, "Insufficient coins");

        user.CoinBalance -= amount;
        user.UpdatedAt = DateTime.UtcNow;

        _db.CoinTransactions.Add(new CoinTransaction
        {
            TxnId = Guid.NewGuid().ToString(),
            UserId = userId,
            ReferenceType = referenceType,
            Amount = -amount,
            BalanceAfter = user.CoinBalance,
            Description = description,
            CreatedAt = DateTime.UtcNow
        });

        await _db.SaveChangesAsync();
        return (true, "Success");
    }

    public async Task AddCoinsAsync(string userId, decimal amount, string referenceType, string? description)
    {
        var user = await GetUserAsync(userId);
        if (user == null) return;

        user.CoinBalance += amount;
        user.UpdatedAt = DateTime.UtcNow;

        _db.CoinTransactions.Add(new CoinTransaction
        {
            TxnId = Guid.NewGuid().ToString(),
            UserId = userId,
            ReferenceType = referenceType,
            Amount = amount,
            BalanceAfter = user.CoinBalance,
            Description = description,
            CreatedAt = DateTime.UtcNow
        });

        await _db.SaveChangesAsync();
    }

    public async Task<(bool Success, string Message)> BuyDigitalGoodAsync(string userId, string goodId)
    {
        var good = await _db.DigitalGoods.FirstOrDefaultAsync(g => g.GoodId == goodId && g.IsActive);
        if (good == null) return (false, "Digital good not found");

        var user = await GetUserAsync(userId);
        if (user == null) return (false, "User not found");

        var totalPrice = good.PriceInCoins;
        var commission = 0m;

        if (good.HasCommission)
        {
            commission = Math.Ceiling(good.PriceInCoins * 0.1m);
            totalPrice += commission;
        }

        if (user.CoinBalance < totalPrice)
            return (false, $"Insufficient coins. Need {totalPrice} but have {user.CoinBalance}");

        user.CoinBalance -= totalPrice;
        user.UpdatedAt = DateTime.UtcNow;

        _db.Purchases.Add(new Purchase
        {
            PurchaseId = Guid.NewGuid().ToString(),
            UserId = userId,
            GoodId = goodId,
            CoinsPaid = good.PriceInCoins,
            AdminCommission = commission,
            CreatedAt = DateTime.UtcNow
        });

        _db.CoinTransactions.Add(new CoinTransaction
        {
            TxnId = Guid.NewGuid().ToString(),
            UserId = userId,
            ReferenceType = "purchase",
            Amount = -totalPrice,
            BalanceAfter = user.CoinBalance,
            Description = $"Bought: {good.Title}",
            CreatedAt = DateTime.UtcNow
        });

        await _db.SaveChangesAsync();
        return (true, $"Purchased successfully! Paid {totalPrice} coins (incl. {commission} commission)");
    }

    public async Task<bool> CanAffordAsync(string userId, decimal amount)
    {
        var user = await GetUserAsync(userId);
        return user != null && user.CoinBalance >= amount;
    }
}
