using System.Net;
using System.Text.Json;

namespace StajyerTakip.Middlewares;

public sealed class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext ctx)
    {
        try
        {
            await _next(ctx);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            if (ctx.Response.HasStarted) throw;

            ctx.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            ctx.Response.ContentType = "application/json; charset=utf-8";

            var payload = new { message = "Beklenmeyen bir hata oluştu." };
            await ctx.Response.WriteAsync(JsonSerializer.Serialize(payload));
        }
    }
}
