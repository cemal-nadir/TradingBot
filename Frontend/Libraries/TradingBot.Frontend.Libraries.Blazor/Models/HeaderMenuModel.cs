using MudBlazor;

namespace TradingBot.Frontend.Libraries.Blazor.Models;

public class HeaderMenuModel
{
    public HeaderMenuModel(string title, List<List<Item>> items)
    {
        Title = title;
        Items = items;
    }

    public string Title { get; }
    public List<List<Item>> Items { get; }


    public string? StartIcon { get; set; }
    public string? EndIcon { get; set; } = Icons.Material.Filled.ArrowDropDown;
    public string? Class { get; set; }
    public string? Style { get; set; }
    public MouseEvent ActivationEvent { get; set; } = MouseEvent.MouseOver;
    public Color Color { get; set; } = Color.Transparent;
    public Size Size { get; set; } = Size.Medium;
    public Variant Variant { get; set; } = Variant.Text;
    public Origin AnchorOrigin { get; set; } = Origin.BottomRight;
    public Origin TransformOrigin { get; set; } = Origin.TopRight;

    public class Item
    {
        public Item(string title, string href, bool addDivider=false)
        {
            Title = title;
            Href = href;
            AddDivider = addDivider;
        }

        public string Title { get; }
        public string Href { get; }

        public string? Target { get; set; }
        public string? Class { get; set; }
        public string? Style { get; set; }
        public string? Icon { get; set; }
        public Color IconColor { get; set; }
        public Size IconSize { get; set; }
        public bool AddDivider { get; set; }
        public bool ForceLoad { get; set; }
        public string? IconCssClass { get; set; }
    }
}