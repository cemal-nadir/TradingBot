using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using TradingBot.Frontend.Web.Blazor.Resources;

namespace TradingBot.Frontend.Web.Blazor.Theme;

public class BreadcrumbsRazor : ComponentBase
{
	[Inject] public NavigationManager NavigationManager { get; set; } = null!;
	[Inject] public IStringLocalizer<Resource> Localizer { get; set; } = null!;
	protected List<BreadcrumbItem>? Items { get; set; }
	[Parameter] public List<BreadcrumbItem>? BreadcrumbItems { get; set; }
	[Parameter] public List<BreadcrumbItem>? AddItems { get; set; }

	protected override void OnParametersSet()
	{
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
		NavigationManager.LocationChanged += NavigationManager_LocationChanged;
	}

	private string CurrentLink;
	private void NavigationManager_LocationChanged(object? sender, Microsoft.AspNetCore.Components.Routing.LocationChangedEventArgs e)
	{
		if (e.Location == CurrentLink) return;
		CurrentLink = e.Location;
		OnParametersSet();
		StateHasChanged();
	}

	private List<BreadcrumbItem> GetBreadcrumbs()
	{
		List<BreadcrumbItem> items = new()
			{ new BreadcrumbItem(Localizer["Home"], NavigationManager.BaseUri.Substring(0,NavigationManager.BaseUri.Length-1), false, BreadCrumbIcons["Home"]) };
		foreach (var link in NavigationManager.Uri.Replace(NavigationManager.BaseUri, "").Split("/").Where(x => !string.IsNullOrEmpty(x)).ToList())
		{
			if (!BreadCrumbIcons.TryGetValue(link, out var icon)) icon = Icons.Material.Filled.Grid3x3;

			items.Add(new BreadcrumbItem(Localizer[link], $"{items.LastOrDefault()?.Href ?? NavigationManager.BaseUri}/{link}", false, icon));
		}

		return items;
	}

	private static readonly Dictionary<string, string> BreadCrumbIcons = new()
	{
		{"Home", Icons.Material.Filled.Home },
		{"Users", Icons.Material.Filled.Groups },
		{"TradingAccounts",Icons.Material.Filled.CurrencyExchange},
		{"Add",Icons.Material.Filled.Add}
	};
}