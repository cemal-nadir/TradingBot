using System.Net;

namespace TradingBot.Frontend.Libraries.Blazor.Responses;

public class ErrorResponse : Response
{
    public ErrorResponse(string message, HttpStatusCode statusCode) : base(false, message, statusCode)
    {
    }

    public ErrorResponse(Exception exception, HttpStatusCode statusCode) : base(false, exception.Message, statusCode)
    {
    }
}

public class ErrorResponse<T> : Response<T>
{
    public ErrorResponse(string message, HttpStatusCode statusCode) : base(false, message, statusCode, default)
    {
    }

    public ErrorResponse(Exception exception, HttpStatusCode statusCode) : base(false, exception.Message, statusCode, default)
    {
    }
}