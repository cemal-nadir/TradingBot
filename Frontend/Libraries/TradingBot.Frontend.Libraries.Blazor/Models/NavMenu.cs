namespace TradingBot.Frontend.Libraries.Blazor.Models;

public class NavMenu : List<NavMenu>
{
    public string? Title { get; set; }
    public string? Url { get; set; }
    public string? Icon { get; set; }
    public bool Expanded { get; set; }
    public List<Item>? Items { get; set; }

    public class Item
    {
        public Item(string title, string url, bool addDivider = false)
        {
            Title = title;
            Url = url;
            AddDivider = addDivider;
        }

        public string Title { get; }
        public string Url { get; }
        public bool AddDivider { get;  }
        public List<Item>? Items { get; set; }
    }
}