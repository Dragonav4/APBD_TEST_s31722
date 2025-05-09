using System.Net;
using System.Text.Json;

namespace APBD_s31722_TEST_TEMPLATE.Exceptions;

public class ApiExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ApiExceptionMiddleware> _logger;

    public ApiExceptionMiddleware(RequestDelegate next, ILogger<ApiExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ApiExceptions ex)
        {
            _logger.LogWarning(ex, "Api Exception(error)");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = ex.StatusCode;
            var payload = ex.ToJson();
            await context.Response.WriteAsync(payload);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception(error)");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var payload = JsonSerializer.Serialize(new {error = "Internal Server Error"});
            await context.Response.WriteAsync(payload);
        }
    }
}