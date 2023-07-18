using System.Net;

namespace TradingBot.Backend.Gateway.API.Responses;

public abstract class Response
{
    protected Response(bool success, string message, HttpStatusCode statusCode)
    {
        Success = success;
        Message = message;
        StatusCode = statusCode;
    }

    public bool Success { get; }
    public string Message { get; }
    
    public HttpStatusCode StatusCode { get; }

}





public abstract class Response<T>:Response
{
   
    public T? Data { get; }

    protected Response(bool success, string message, HttpStatusCode statusCode, T? data) : base(success, message, statusCode)
    {
        Data = data;
    }
}