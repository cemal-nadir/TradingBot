namespace TradingBot.Frontend.Libraries.Blazor.Models;

public class PagedList<T>
{
    public List<T>? Data { get; set; }

    public int CurrentPage { get; set; }

    public int TotalPage { get; set; }

    public int PageSize { get; set; }

    public bool First { get; set; }

    public bool Next { get; set; }

    public bool Prior { get; set; }

    public bool Last { get; set; }

    public int TotalCount { get; set; }

    public string? LastCachedDate { get; set; }
}