using AkkuAppsAi.Services;

namespace AkkuAppsAi.Middleware;

public class VisitTrackingMiddleware
{
    private readonly RequestDelegate _next;

    public VisitTrackingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, AnalyticsService analyticsService)
    {
        if (context.Request.Method == "GET" && !context.Request.Path.StartsWithSegments("/css")
            && !context.Request.Path.StartsWithSegments("/js")
            && !context.Request.Path.StartsWithSegments("/lib")
            && !context.Request.Path.StartsWithSegments("/favicon.ico"))
        {
            var ip = context.Connection.RemoteIpAddress?.ToString();
            var sessionId = context.Request.Cookies["akku_session"];
            if (string.IsNullOrEmpty(sessionId))
            {
                sessionId = Guid.NewGuid().ToString();
                context.Response.Cookies.Append("akku_session", sessionId, new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(30),
                    HttpOnly = true,
                    SameSite = SameSiteMode.Lax
                });
            }

            var shouldTrack = await analyticsService.ShouldTrackVisitAsync(ip, sessionId);
            if (shouldTrack)
            {
                var url = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
                var userAgent = context.Request.Headers.UserAgent.ToString();
                var timeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time").Id;

                await analyticsService.TrackVisitAsync(ip, sessionId, url, userAgent,
                    null, null, timeZone);
            }
        }

        await _next(context);
    }
}

public static class VisitTrackingMiddlewareExtensions
{
    public static IApplicationBuilder UseVisitTracking(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<VisitTrackingMiddleware>();
    }
}
