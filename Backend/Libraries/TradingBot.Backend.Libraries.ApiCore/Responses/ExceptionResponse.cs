using System.Net;

namespace TradingBot.Backend.Libraries.ApiCore.Responses;

public class ExceptionResponse
{
    public HttpStatusCode StatusCode { get; set; }
    public string? ExceptionType { get; set; }
    public string? Message { get; set; }
    public string? StackTrace { get; set; }
}