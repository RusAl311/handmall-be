using System.Net;
using System.Text.Json;

using NotFoundException = Application.Common.Exceptions.NotFoundException;

namespace WebApi.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware (RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke (HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleException(httpContext, ex);
        }
    }

    private static Task HandleException(HttpContext httpContext, Exception ex)
    {
        HttpStatusCode status;
        var stackTrace = string.Empty;
        string message = "";

        var exceptionType = ex.GetType();

        if(exceptionType == typeof(NotFoundException)) 
        {
            message = ex.Message;
            status = HttpStatusCode.BadRequest;
            stackTrace = ex.StackTrace;
        }
        else
        {
            message = ex.Message;
            status = HttpStatusCode.InternalServerError;
            stackTrace = ex.StackTrace;
        }

        var exceptionResult = JsonSerializer.Serialize(new {error = message, stackTrace});
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int) status;

        return httpContext.Response.WriteAsync(exceptionResult);
    }
}