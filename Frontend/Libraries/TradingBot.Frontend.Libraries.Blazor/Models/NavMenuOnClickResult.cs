namespace TradingBot.Frontend.Libraries.Blazor.Models;

public class NavMenuOnClickResult
{
    public NavMenuOnClickResult(string id, string command)
    {
        Id = id;
        Command = command;
    }

    public string Id { get; }
    public string Command { get; }
}