using System.Net;
using System.Security;
using System.Security.Authentication;
using System.Text.Json;
using TradingBot.Backend.Services.Identity.Api.Exceptions;

namespace TradingBot.Backend.Services.Identity.Api.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IWebHostEnvironment env)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, env);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception, IHostEnvironment env)
    {
        HttpStatusCode status;
        string message;
        var stackTrace = string.Empty;

        var exceptionType = exception.GetType();
        if (exceptionType == typeof(BadRequestException))
        {
            message = exception.InnerException?.Message ?? exception.Message;
            status = HttpStatusCode.BadRequest;
        }
        else if (exceptionType == typeof(DbException))
        {
            message = exception.InnerException?.Message ?? exception.Message;
            status = HttpStatusCode.BadRequest;
        }
        else if (exceptionType == typeof(ValidationException))
        {
            message = exception.InnerException?.Message ?? exception.Message;
            status = HttpStatusCode.BadRequest;
        }
        else if (exceptionType == typeof(NotFoundException))
        {
            message = exception.InnerException?.Message ?? exception.Message;
            status = HttpStatusCode.NotFound;
        }
        else if (exceptionType == typeof(DbNullException))
        {
            message = exception.InnerException?.Message ?? exception.Message;
            status = HttpStatusCode.NotFound;
        }
        else if (exceptionType == typeof(AuthenticationException))
        {
            message = exception.InnerException?.Message ?? exception.Message;
            status = HttpStatusCode.Unauthorized;
        }
        else if (exceptionType == typeof(SecurityException))
        {
            message = exception.InnerException?.Message ?? exception.Message;
            status = HttpStatusCode.Forbidden;
        }
        else
        {
            status = HttpStatusCode.InternalServerError;
            message = exception.InnerException?.Message ?? exception.Message;
            if (env.IsEnvironment("Development"))
                stackTrace = exception.StackTrace;
        }


        var result = JsonSerializer.Serialize(new ExceptionResponse
        {
            StatusCode = status,
            ExceptionType = exceptionType.Name,
            Message = ParseToMessage(message),
            StackTrace = stackTrace
        });

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)status;

        return context.Response.WriteAsync(result);
    }
    private static string ParseToMessage(string message)
    {
        var result = message;
        if (message.Contains("insert duplicate key"))
        {
            result = ErrorDefaults.DbException.Duplicate;
        }
        else if (message.Contains("REFERENCE constraint \"FK_"))
        {
            var firstIndex = message.IndexOf("REFERENCE constraint \"FK_", StringComparison.Ordinal);
            var first = message.Substring(firstIndex, message.Length - firstIndex).Replace("REFERENCE constraint \"FK_", "").Split("_")[0];
            return $"{ErrorDefaults.DbException.ReferenceConstraint}|{first}";
        }
        return result;
    }
}