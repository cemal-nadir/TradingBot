using System.Net;

namespace TradingBot.Backend.Services.Identity.Api.Middlewares;

public class ExceptionResponse
{
    public HttpStatusCode StatusCode { get; set; }
    public string? ExceptionType { get; set; }
    public string? Message { get; set; }
    public string? StackTrace { get; set; }
}