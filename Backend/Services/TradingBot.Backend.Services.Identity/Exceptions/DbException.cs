
namespace TradingBot.Backend.Services.Identity.Api.Exceptions
{
  public class DbException : Exception
  {
    public DbException(string message)
      : base(message)
    {
    }
  }
}
