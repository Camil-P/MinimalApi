using System.Net;
using System.Text.Json;

namespace MinimalApiTemplate.Middleware;

public class ErrorHandlingMiddleware
{
    private const string errorMessage = "Error has occurred while processing your request, please try again.";
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    private readonly IHostEnvironment _hostEnvironment;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger, IHostEnvironment hostEnvironment)
    {
        _next = next;
        _logger = logger;
        _hostEnvironment = hostEnvironment;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        switch (ex)
        {
            case BadHttpRequestException badRequestEx:
                context.Response.StatusCode = badRequestEx.StatusCode;
                string errorMessage = badRequestEx.Message;
                return context.Response.WriteAsync(errorMessage);

            default:
                _logger.LogError(ex, ex.ToString());
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                //if (_hostEnvironment.IsDevelopment())
                //{
                //}
                return context.Response.WriteAsync(JsonSerializer.Serialize(
                    new { error = $"{ex.Message} + {ex.StackTrace} + {ex.Source}" }));
                //return context.Response.WriteAsync(JsonSerializer.Serialize(new { error = errorMessage }));
        }
    }
}
