using IdentityModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using MudBlazor;
using TradingBot.Frontend.Libraries.Blazor.Models;
using TradingBot.Frontend.Web.Blazor.Components.Bases;
using TradingBot.Frontend.Web.Blazor.Dtos.Users;
using TradingBot.Frontend.Web.Blazor.Services.Abstract.User;
using TradingBot.Frontend.Web.Blazor.Theme.Components;

namespace TradingBot.Frontend.Web.Blazor.Pages.TradingAccounts
{
	public class IndicatorsRazor:BaseComponent
	{
		
		[Inject] private ITradingAccountService TradingAccountService { get; set; } = null!;
		[Inject] private Env Env { get; set; } = null!;
		[Parameter] public string? TradingAccountId { get; set; }
		[CascadingParameter] public Task<AuthenticationState> AuthStateTask { get; set; } = null!;

		protected List<IndicatorDto>?Entities { get; set; }
		protected List<IndicatorDto>?SelectedEntities { get; set; }
		protected void SelectedItemsChanged(HashSet<IndicatorDto> items)
		{
			SelectedEntities = items.ToList();
		}
		protected override async Task OnInitializedAsync()
		{
			await GetAllAsync();
		}
		protected async Task DeleteAsync(string id)
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
				StateHasChanged();
				var tradingAccount=await TradingAccountService.GetAsync(TradingAccountId??"");
				if (!tradingAccount.Success||tradingAccount.Data is null)
				{
					Snackbar.Add(tradingAccount.Message, Severity.Error);
					Navigation.NavigateTo("/TradingAccounts");
					return;
				}

				var updateResponse=await TradingAccountService.UpdateAsync(TradingAccountId??"", new TradingAccountDto
				{
					ApiKey = tradingAccount.Data.ApiKey,
					BalanceSettings = tradingAccount.Data.BalanceSettings,
					IsActive = tradingAccount.Data.IsActive,
					Name = tradingAccount.Data.Name,
					Platform = tradingAccount.Data.Platform,
					SecretKey = tradingAccount.Data.SecretKey,
					UserId = tradingAccount.Data.UserId,
					Indicators = tradingAccount.Data.Indicators.Where(x => x.Id != id).ToList()
				});
				
				Loading = false;
				StateHasChanged();
				if (updateResponse.Success)
				{
					await GetAllAsync();
				}
				else Snackbar.Add(updateResponse.Message, Severity.Error);
			}
		}

		protected  async Task EditAsync(string id)
		{
			string title = Localizer[$"{nameof(Indicator)}"];
			try
			{
				var entity = await TradingAccountService.GetAsync(TradingAccountId??"");
				if (entity is { Success: true, Data: not null } && entity.Data.Indicators
					    .FirstOrDefault(x => x.Id == id)?.GetType().GetProperty("Name")
					    ?.GetValue(entity.Data.Indicators.FirstOrDefault(x => x.Id == id)) != null)
				{
					title =
						$"{Localizer[$"{nameof(Indicator)}"]} - {entity.Data.Indicators.FirstOrDefault(x => x.Id == id)?.GetType().GetProperty("Name")?.GetValue(entity.Data.Indicators.FirstOrDefault(x => x.Id == id))}";

				}

			}
			catch (Exception)
			{
				//
			}


			var dialog = await DialogService.ShowAsync<Indicator>(
				title,
				new DialogParameters { { "Id", id },{"TradingAccountId",TradingAccountId} },
				new DialogOptions
				{
					MaxWidth = MaxWidth.Small,
					CloseButton = true,
					CloseOnEscapeKey = true,

				}
			);
			if (dialog?.Result != null && !(await dialog.Result).Canceled) await GetAllAsync();
		}

		private async Task GetAllAsync(CancellationToken cancellationToken=default)
		{
			Loading = true;
			StateHasChanged();
			var authState = await AuthStateTask;

			if (TradingAccountId is null)
			{
				Navigation.NavigateTo("/TradingAccounts");
				return;
			}

			var tradingAccountResult = await TradingAccountService.GetAsync(TradingAccountId, cancellationToken);

			if (!tradingAccountResult.Success)
			{
				Snackbar.Add(tradingAccountResult.Message, Severity.Error);
				Navigation.NavigateTo("/TradingAccounts");
				return;
			}

			if (!authState.User.IsInRole("admin") && tradingAccountResult.Data?.UserId !=
			    authState.User.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Subject)?.Value)
			{
				Navigation.NavigateTo("/TradingAccounts");
				return;
			}

			Entities = tradingAccountResult.Data?.Indicators;
			Loading = false;
			StateHasChanged();
		}
		protected async Task AddNew()
		{
			var dialog = await DialogService.ShowAsync<Indicator>(
				Localizer[$"{nameof(Indicator)}"],
				new DialogParameters { { "Id", null },{"TradingAccountId",TradingAccountId} },
				new DialogOptions
				{
					MaxWidth = MaxWidth.Small,
					CloseButton = true,
					CloseOnEscapeKey = true,
				}
			);
			if (dialog?.Result != null && !(await dialog.Result).Canceled) await GetAllAsync();
		}
	}
}
