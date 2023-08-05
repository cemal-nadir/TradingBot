using IdentityModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using TradingBot.Frontend.Web.Blazor.Components.Bases;
using TradingBot.Frontend.Web.Blazor.Dtos.Users;
using TradingBot.Frontend.Web.Blazor.Services.Abstract.User;

namespace TradingBot.Frontend.Web.Blazor.Pages.TradingAccounts
{
	public class IndicatorRazor:BaseComponent
	{
		[Inject] private ITradingAccountService TradingAccountService { get; set; } = null!;
		[Inject] protected IndicatorValidator Validator { get; set; } = null!;
		[CascadingParameter] public Task<AuthenticationState> AuthStateTask { get; set; } = null!;
		[CascadingParameter] public MudDialogInstance? MudDialog { get; set; }
		[Parameter] public string? Id { get; set; }
		[Parameter] public string? TradingAccountId { get; set; }
		protected IndicatorDto Entity { get; set; } = new();
		protected MudForm Form { get; set; } = new();
		protected override async Task OnInitializedAsync()
		{
			await GetAllAsync();
		}
		private async Task GetAllAsync()
		{
			var authState = await AuthStateTask;

			var tradingAccount = await TradingAccountService.GetAsync(TradingAccountId ?? "");

			if (!tradingAccount.Success || tradingAccount.Data is null)
			{
				Snackbar.Add(tradingAccount.Message, Severity.Error);
				Navigation.NavigateTo("/TradingAccounts");
				return;
			}

			if (!authState.User.IsInRole("admin") &&
			    authState.User.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Subject)?.Value !=
			    tradingAccount.Data?.UserId)
			{
				Navigation.NavigateTo("/TradingAccounts");
				return;
			}
			if (Id is "Add") Id = null;
			if (Id is null)
			{
				Entity = new();
			}
			else
			{
				Entity = tradingAccount.Data?.Indicators.FirstOrDefault(x => x.Id == Id) ?? new();
			}
		}
		protected async Task SaveChangesAsync()
		{
			await Form.Validate();
			if (Form.IsValid)
			{
				Loading = true;
				StateHasChanged();
				if (Id is null) await InsertAsync();
				else await UpdateAsync();
				Loading = false;
				StateHasChanged();
			}
			else
			{
				Snackbar.Add(Localizer["PleaseCheckForm"], Severity.Warning);
			}
		}
		private async Task InsertAsync()
		{
			var tradingAccount = await TradingAccountService.GetAsync(TradingAccountId??"");
			if (tradingAccount is { Success: false, Data: null })
			{
				Navigation.NavigateTo("/TradingAccounts");
				return;
			}

			var newData = tradingAccount.Data ?? new();
			newData.Indicators.Add(Entity);
			var updateResult=await TradingAccountService.UpdateAsync(TradingAccountId??"", newData);
			if (updateResult.Success)
			{
				Snackbar.Add(Localizer["SaveChangesSuccess"], Severity.Success);
				MudDialog?.Close();
			}
			else Snackbar.Add(updateResult.Message, Severity.Error);
		}
		private async Task UpdateAsync()
		{
			var tradingAccount = await TradingAccountService.GetAsync(TradingAccountId ?? "");
			if (tradingAccount is { Success: false, Data: null })
			{
				Navigation.NavigateTo("/TradingAccounts");
				return;
			}

			var newData = tradingAccount.Data ?? new();
			newData.Indicators.Remove(newData.Indicators.FirstOrDefault(x => x.Id == Id) ?? new());
			newData.Indicators.Add(Entity);
			var updateResult = await TradingAccountService.UpdateAsync(TradingAccountId ?? "", newData);
			if (updateResult.Success)
			{
				Snackbar.Add(Localizer["SaveChangesSuccess"], Severity.Success);
				MudDialog?.Close();
			}
			else Snackbar.Add(updateResult.Message, Severity.Error);
		}
	}
}
