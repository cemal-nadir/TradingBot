namespace TradingBot.Frontend.Libraries.Blazor.Models;

public class Breadcrumbs<TKey>
{
    public TKey Id { get; set; } = default!;
    public string Description { get; set; } = string.Empty;
}