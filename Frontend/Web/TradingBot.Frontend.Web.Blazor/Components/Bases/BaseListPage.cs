using CNG.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using TradingBot.Frontend.Libraries.Blazor.Models;
using TradingBot.Frontend.Libraries.Blazor.Repositories;
using TradingBot.Frontend.Libraries.Blazor.Services;
using TradingBot.Frontend.Libraries.Blazor.Signatures;
using TradingBot.Frontend.Web.Blazor.Theme.Components;
using static IdentityModel.OidcConstants;
using static MudBlazor.CategoryTypes;

namespace TradingBot.Frontend.Web.Blazor.Components.Bases;
public class BaseListPage<TDto, TListDto, TKey, TService, TDetailPage, TValidator> : BaseComponent
	where TDto : class, IDto, new()
	where TListDto : IListDto<TKey>, new()
	where TKey : IEquatable<TKey>
	where TService : IServiceRepository<TKey, TDto, TListDto>
	where TValidator : ValidatorBase<TDto>
	where TDetailPage : BaseDetailPage<TDto, TListDto, TKey, TService, TValidator>
{
	[Inject] protected TService Service { get; set; } = default!;

	protected MaxWidth MaxWidth = MaxWidth.Medium;
	protected List<TListDto>? SelectedEntities { get; set; }
	protected List<TListDto>? Entities { get; set; }
	protected override async Task OnInitializedAsync()
	{
		await GetAllAsync();
	}
	protected virtual async Task NavMenuOnClickAsync(NavMenuOnClickResult result)
	{
		switch (result.Command)
		{
			case "Create":
				await CreateAsync();
				break;
			case "Refresh":
			case "RemoveCache":
				await RemoveCacheAsync();
				await GetAllAsync();
				break;
			case "Delete":
				await DeleteAsync(result.Id.ChangeType<TKey>());
				break;
			case "Edit":
				await EditAsync(result.Id.ChangeType<TKey>());
				break;
		}
	}
	protected void SelectedItemsChanged(HashSet<TListDto> items)
	{
		SelectedEntities = items.ToList();
	}
	#region Service Methods

	protected virtual async Task GetAllAsync(CancellationToken cancellationToken = default)
	{
		Loading = true;
		StateHasChanged();

		var response = await Service.GetAllAsync(cancellationToken);
		Entities = response.Data;

		Loading = false;
	}

	protected virtual async Task DeleteAsync(TKey id, CancellationToken cancellationToken = default)
	{
		var dialog = await DialogService.ShowAsync<ConfirmationDialog>(
			title: Localizer["Delete"],
			parameters: new DialogParameters
			{
				{ "ContentText", $"{Localizer["Delete_Confirm_Content"]}" },
				{ "OkButtonText", $"{Localizer["Delete"]}" },
				{ "CancelButtonText", $"{Localizer["Cancel"]}" },
				{ "Color", Color.Error }
			},
			new DialogOptions()
			{
				MaxWidth = MaxWidth.Small,
				CloseButton = true,
				CloseOnEscapeKey = true,
			});
		if (dialog?.Result != null && !(await dialog.Result).Canceled)
		{
			Loading = true;
			var response = await Service.DeleteAsync(id, cancellationToken);
			Loading = false;
			if (response.Success)
			{
				await GetAllAsync(cancellationToken);
			}
			else Snackbar.Add(response.Message, Severity.Error);
		}
	}

	protected virtual async Task CreateAsync(CancellationToken cancellationToken = default)
	{
		var dialog = await DialogService.ShowAsync<TDetailPage>(
			Localizer[$"{typeof(TDetailPage).Name}"],
			new DialogParameters { { "Id", null } },
			new DialogOptions
			{
				MaxWidth = MaxWidth,
				CloseButton = true,
				CloseOnEscapeKey = true,

			}
		);
		if (dialog?.Result != null && !(await dialog.Result).Canceled) await GetAllAsync(cancellationToken);
	}

	protected virtual async Task EditAsync(TKey id, CancellationToken cancellationToken = default)
	{
		string title = Localizer[$"{typeof(TDetailPage).Name}"];
		try
		{
			var entity = await Service.GetAsync(id, cancellationToken);
			if (entity is { Success: true, Data: not null } && entity.Data?.GetType().GetProperty("Name")?.GetValue(entity.Data) != null)
			{
				title = $"{Localizer[$"{typeof(TDetailPage).Name}"]} - {entity.Data?.GetType().GetProperty("Name")?.GetValue(entity.Data)}";
			}
		}
		catch (Exception)
		{
			//
		}


		var dialog = await DialogService.ShowAsync<TDetailPage>(
			title,
			new DialogParameters { { "Id", id } },
			new DialogOptions
			{
				MaxWidth = MaxWidth,
				CloseButton = true,
				CloseOnEscapeKey = true,

			}
		);
		if (dialog?.Result != null && !(await dialog.Result).Canceled) await GetAllAsync(cancellationToken);
	}

	protected virtual async Task DeleteRangeAsync()
	{
		if (SelectedEntities is null) return;
		var dialog = await DialogService.ShowAsync<ConfirmationDialog>(
			title: Localizer["Delete"],
			parameters: new DialogParameters
			{
				{ "ContentText", $"{Localizer["Delete_Confirm_Content"]}" },
				{ "OkButtonText", $"{Localizer["Delete"]}" },
				{ "CancelButtonText", $"{Localizer["Cancel"]}" },
				{ "Color", Color.Error }
			});
		if (dialog?.Result != null && !(await dialog.Result).Canceled)
		{
			Loading = true;
			var response = await Service.DeleteRangeAsync(SelectedEntities.Select(x=>x.Id));
			Loading = false;
			if (response.Success)
			{
				SelectedEntities = null;
				await GetAllAsync();
			}
			else Snackbar.Add(response.Message, Severity.Error);
		}
	}

	protected virtual async Task RemoveCacheAsync(CancellationToken cancellationToken = default) =>
		await Service.RemoveCacheAsync(cancellationToken).ConfigureAwait(false);

	#endregion
}