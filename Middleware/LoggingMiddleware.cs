namespace UserManagementAPI.Middleware;

public class LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // Log the request
        logger.LogInformation("HTTP Request: {Method} {Path}", context.Request.Method, context.Request.Path);

        // Call the next middleware in the pipeline
        await next(context);

        // Log the response
        logger.LogInformation("HTTP Response: {StatusCode}", context.Response.StatusCode);
    }
}