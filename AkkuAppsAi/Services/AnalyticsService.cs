using AkkuAppsAi.Data;
using AkkuAppsAi.Models;
using Microsoft.EntityFrameworkCore;

namespace AkkuAppsAi.Services;

public class AnalyticsService
{
    private readonly AppDbContext _db;

    public AnalyticsService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<int> GetTotalVisitorsAsync()
    {
        return await _db.PageVisits
            .Select(v => v.IPAddress)
            .Distinct()
            .CountAsync();
    }

    public async Task<int> GetVisitorsTodayAsync()
    {
        var todayStart = DateTime.UtcNow.Date;
        return await _db.PageVisits
            .Where(v => v.VisitedAt >= todayStart)
            .Select(v => v.IPAddress)
            .Distinct()
            .CountAsync();
    }

    public async Task<List<(DateTime Date, int Count)>> GetVisitorsByDateAsync(int days = 30)
    {
        var start = DateTime.UtcNow.Date.AddDays(-days);
        var data = await _db.PageVisits
            .Where(v => v.VisitedAt >= start)
            .GroupBy(v => v.VisitedAt.Date)
            .Select(g => new { Date = g.Key, Count = g.Select(v => v.IPAddress).Distinct().Count() })
            .OrderBy(g => g.Date)
            .ToListAsync();

        return data.Select(d => (d.Date, d.Count)).ToList();
    }

    public async Task<List<(string Country, int Count)>> GetVisitorsByCountryAsync()
    {
        var data = await _db.PageVisits
            .Where(v => v.Country != null)
            .GroupBy(v => v.Country)
            .Select(g => new { Country = g.Key, Count = g.Select(v => v.IPAddress).Distinct().Count() })
            .OrderByDescending(g => g.Count)
            .ToListAsync();

        return data.Select(d => (d.Country!, d.Count)).ToList();
    }

    public async Task<List<(string TimeZone, int Count)>> GetVisitorsByTimeZoneAsync()
    {
        var data = await _db.PageVisits
            .Where(v => v.TimeZone != null)
            .GroupBy(v => v.TimeZone)
            .Select(g => new { TimeZone = g.Key, Count = g.Select(v => v.IPAddress).Distinct().Count() })
            .OrderByDescending(g => g.Count)
            .ToListAsync();

        return data.Select(d => (d.TimeZone!, d.Count)).ToList();
    }

    public async Task<bool> ShouldTrackVisitAsync(string? ip, string? sessionId)
    {
        if (string.IsNullOrEmpty(ip) && string.IsNullOrEmpty(sessionId))
            return true;

        var since = DateTime.UtcNow.AddHours(-24);
        var exists = await _db.PageVisits
            .AnyAsync(v => v.VisitedAt >= since
                && ((ip != null && v.IPAddress == ip)
                    || (sessionId != null && v.SessionId == sessionId)));
        return !exists;
    }

    public async Task TrackVisitAsync(string? ip, string? sessionId, string? url, string? userAgent,
        string? country, string? city, string? timeZone)
    {
        var visit = new PageVisit
        {
            IPAddress = ip,
            SessionId = sessionId,
            Url = url,
            UserAgent = userAgent,
            Country = country,
            City = city,
            TimeZone = timeZone,
            VisitedAt = DateTime.UtcNow
        };
        _db.PageVisits.Add(visit);
        await _db.SaveChangesAsync();
    }
}
