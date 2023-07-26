using MudBlazor;

namespace TradingBot.Frontend.Libraries.Blazor.Models;

public class MenuModel
{
    public Menu MenuHeader { get; set; } = new();

    public NavMenu NavMenuHeader { get; set; } = new();

    public class NavMenu
    {
        public string Width { get; set; } = "150px";
        public int Elevation { get; set; } = 0;
        public Color Color { get; set; } = Color.Inherit;
        public string Class { get; set; } = string.Empty;

        public List<NavMenuItem> Items { get; set; } = new();
    }

    public class NavMenuItem
    {
        public NavMenuItem(string id, string title, string onClickCommand, string icon, bool addDivider = false)
        {
            Id = id;
            Title = title;
            OnClickCommand = onClickCommand;
            Icon = icon;
            AddDivider = addDivider;
        }

        public NavMenuItem(string title, string onClickCommand, string icon, bool addDivider = false)
        {
            Id = string.Empty;
            Title = title;
            OnClickCommand = onClickCommand;
            Icon = icon;
            AddDivider = addDivider;
        }

        public string Id { get; }
        public string Title { get; }
        public string OnClickCommand { get; }
        public string Icon { get; }
        public bool AddDivider { get; }
        public string? Href { get; set; }
        public string ActiveClass { get; set; } = "active";
        public string Class { get; set; } = string.Empty;
        public Color IconColor { get; set; } = Color.Inherit;
        public bool Disabled { get; set; }
        public bool ForceLoad { get; set; }

        public List<NavMenuItem>? Items { get; set; }
    }

    public class Menu
    {
        public string Title { get; set; } =string.Empty;
        public string Icon { get; set; } = Icons.Material.Filled.MoreVert;
        public Variant Variant { get; set; } = Variant.Text;
        public Origin AnchorOrigin { get; set; } = Origin.BottomRight;
        public Origin TransformOrigin { get; set; } = Origin.TopRight;
    }

    public class Button
    {
        public string Text { get; set; } = string.Empty;
        public Variant Variant { get; set; } = Variant.Text;
        public Color Color { get; set; } = Color.Inherit;
        public string StartIcon { get; set; } = Icons.Material.Filled.MoreVert;
        public string EndIcon { get; set; } = Icons.Material.Filled.KeyboardArrowDown;
    }
}