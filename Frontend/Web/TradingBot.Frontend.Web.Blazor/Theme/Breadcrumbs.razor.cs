using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using TradingBot.Frontend.Libraries.Blazor.Models;
using TradingBot.Frontend.Web.Blazor.Resources;

namespace TradingBot.Frontend.Web.Blazor.Theme;

public class BreadcrumbsRazor : ComponentBase
{
    [Inject] public NavigationManager? NavigationManager { get; set; }
    [Inject] public IStringLocalizer<Resource> Localizer { get; set; } = null!;
    protected List<BreadcrumbItem>? Items { get; set; }
    [Parameter] public List<BreadcrumbItem>? BreadcrumbItems { get; set; }
    [Parameter] public List<BreadcrumbItem>? AddItems { get; set; }

    private List<HeaderMenuModel.Item>? MenuItems { get; set; }
    protected override void OnParametersSet()
    {
        MenuItems = new();


        Items = new List<BreadcrumbItem>();
        if (BreadcrumbItems != null)
        {
            Items.AddRange(BreadcrumbItems);
        }
        else if (AddItems != null)
        {
            Items = GetBreadcrumbs();
            Items.AddRange(AddItems);
        }
        else
        {
            Items = GetBreadcrumbs();
        }
    }

    private List<BreadcrumbItem> GetBreadcrumbs()
    {
        var url = NavigationManager?.Uri.Replace(NavigationManager?.BaseUri ?? "", "").Split("/").FirstOrDefault();

        var menu = MenuItems!.FirstOrDefault(x =>  x.Href == url);
        var result = new List<BreadcrumbItem> { new("", "/", false, Icons.Material.Filled.Home) };
        if (menu == null)
        {
            //result.Add(new BreadcrumbItem("Home", "/"));
            return result;
        }

        result.Add(new BreadcrumbItem(Localizer[menu.Title], "", true));
        result.AddRange(MenuItems!.Where(x => x.Href == url).Select(item => new BreadcrumbItem(Localizer[item.Title], item.Href)));

        return result;
    }
}