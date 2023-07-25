#nullable enable
namespace TradingBot.Backend.Services.Identity.Api.Exceptions
{
  public class BadRequestException : Exception
  {
    public BadRequestException(string message)
      : base(message)
    {
    }
  }
}
