using Microsoft.AspNetCore.Components;
using MudBlazor;
using TradingBot.Frontend.Libraries.Blazor.Repositories;
using TradingBot.Frontend.Libraries.Blazor.Services;
using TradingBot.Frontend.Libraries.Blazor.Signatures;

namespace TradingBot.Frontend.Web.Blazor.Components.Bases;

public class BaseDetailPage<TDto, TListDto, TKey, TService, TValidator> : BaseComponent
    where TDto : class, IDto, new()
    where TListDto : IListDto<TKey>, new()
    where TKey : IEquatable<TKey>
    where TService : IServiceRepository<TKey, TDto, TListDto>
    where TValidator: ValidatorBase<TDto>
{
    #region Paramters

    [CascadingParameter] public MudDialogInstance? MudDialog { get; set; }
    [Parameter] public TKey? Id { get; set; }
    protected MudForm Form { get; set; } = new();
    [Inject] protected TValidator Validator { get; set; } = null!;
    #endregion

    protected bool IsNew => Id == null || string.IsNullOrEmpty(Id.ToString()) || Id.ToString() == Guid.Empty.ToString();

    #region Private Methods

    protected void Cancel() => MudDialog?.Cancel();

    protected override async Task OnInitializedAsync()
    {
        await GetAsync();
        StateHasChanged();
    }

    #endregion

    #region Injections

    [Inject] protected TService Service { get; set; } = default!;

    #endregion

    #region Properties

    protected TDto Entity { get; set; } = new();
    protected bool Processing { get; set; }

    #endregion

    #region Service Methods

    protected virtual async Task SaveChangesAsync()
    {
        await Form.Validate();
        if (Form.IsValid)
        {
            Processing = true;
            if (IsNew) await InsertAsync();
            else await UpdateAsync();
            Processing = false;
            Snackbar.Add(Localizer["SaveChangesSuccess"], Severity.Success);
        }
        else
        {
	        Snackbar.Add(Localizer["PleaseCheckForm"], Severity.Warning);
		}
	}
    protected virtual async Task GetAsync(CancellationToken cancellationToken = default)
    {
        if (!IsNew)
        {
            if (Id != null)
            {
                Processing = true;
                var response = await Service.GetAsync(Id, cancellationToken);
                Processing = false;
                if (response.Success) Entity = response.Data ?? new TDto();
                else Snackbar.Add(response.Message, Severity.Error);
            }
        }
    }

    protected virtual async Task InsertAsync(CancellationToken cancellationToken = default)
    {
        Processing = true;
        var response = await Service.InsertAsync(Entity, cancellationToken);
        Processing = false;
        if (response.Success) MudDialog?.Close();
        else Snackbar.Add(response.Message, Severity.Error);
    }

    protected virtual async Task UpdateAsync(CancellationToken cancellationToken = default)
    {
        if (Id != null)
        {
            Processing = true;
            var response = await Service.UpdateAsync(Id, Entity, cancellationToken);
            Processing = false;
            if (response.Success) MudDialog?.Close();
            else Snackbar.Add(response.Message, Severity.Error);
        }
        else
        {
            Snackbar.Add("Id is null", Severity.Error);
        }
    }

    #endregion
}