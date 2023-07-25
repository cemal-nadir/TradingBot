using System.Net;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using TradingBot.Frontend.Libraries.Blazor.Repositories;
using TradingBot.Frontend.Libraries.Blazor.Signatures;
using TradingBot.Frontend.Web.Blazor.Theme.Components;

namespace TradingBot.Frontend.Web.Blazor.Components.Bases;
public class BaseListPage<TDto, TListDto, TKey, TService, TDetailPage> : BaseComponent
    where TDto : IDto, new()
    where TListDto : IListDto<TKey>, new()
    where TKey : IEquatable<TKey>
    where TService : IServiceRepository<TKey, TDto, TListDto>
    where TDetailPage : BaseDetailPage<TDto, TListDto, TKey, TService>
{
    [Inject] protected TService Service { get; set; } = default!;

    protected MaxWidth MaxWidth = MaxWidth.Small;
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

    #region Service Methods

    protected virtual async Task GetAllAsync(CancellationToken cancellationToken = default)
    {
        Loading = true;
        StateHasChanged();

        var response = await Service.GetAllAsync(cancellationToken);

        if (response.Success)
        {
            Entities = response.Data;
        }
        else
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized) Navigation.NavigateTo("/login");
            else Snackbar.Add($"{response.Message} ({response.StatusCode})", Severity.Error);
        }
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
                MaxWidth = MaxWidth
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
            if (entity.Success && entity.Data != null)
            {
                title = entity.Data?.GetType().GetProperty("Description")?.GetValue(entity.Data)?.ToString() ??
                        Localizer[$"{typeof(TDetailPage).Name}"];
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

            }
        );
        if (dialog?.Result != null && !(await dialog.Result).Canceled) await GetAllAsync(cancellationToken);
    }

    protected virtual async Task DeleteRangeAsync()
    {
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
            //Loading = true;
            //var response = await Service.DeleteRangeAsync(listOfId);
            //Loading = false;
            //if (response.Success)
            //{
            //    await GetAllAsync();
            //}
            //else Snackbar.Add(response.Message, Severity.Error);
        }
    }

    protected virtual async Task RemoveCacheAsync(CancellationToken cancellationToken = default) =>
        await Service.RemoveCacheAsync(cancellationToken).ConfigureAwait(false);

    #endregion
}