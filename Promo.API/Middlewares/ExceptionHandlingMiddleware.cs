using Promo.Domain.Results;
using System.Net;
using ILogger = Serilog.ILogger;

namespace Promo.Api.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger logger)
    { 
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            _logger.Information($"Request: {context.Request.Method} {context.Request.Path}");
            await _next(context);
            _logger.Information($"Response: {context.Response.StatusCode}");
        }
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }
    }

    private async Task HandleException(HttpContext httpContext, Exception exception)
    {
        _logger.Error(exception, $"Exception while processing request: {exception.Message}");
        var response = new Result()
        {
            ErrorText = exception.Message,
            ErrorCode = (int)HttpStatusCode.InternalServerError
        };

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = response.ErrorCode.Value;
        await httpContext.Response.WriteAsJsonAsync(response);
    }
}
