
namespace TradingBot.Backend.Services.Identity.Api.Exceptions
{
  public class DbNullException : Exception
  {
    public DbNullException(string message)
      : base(message)
    {
    }
  }
}
