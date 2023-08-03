using IdentityModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using TradingBot.Frontend.Web.Blazor.Components.Bases;
using TradingBot.Frontend.Web.Blazor.Dtos.Identity;
using TradingBot.Frontend.Web.Blazor.Dtos.Users;
using TradingBot.Frontend.Web.Blazor.Services.Abstract.TradingPlatforms;
using TradingBot.Frontend.Web.Blazor.Services.Abstract.User;

namespace TradingBot.Frontend.Web.Blazor.Pages.TradingAccounts
{
	public class TradingAccountRazor : BaseDetailPage<TradingAccountDto, TradingAccountsDto, string, ITradingAccountService, TradingAccountValidator>
	{
		[Inject] private IUserService UserService { get; set; } = null!;
		[Inject] private ITradingPlatformService TradingPlatformService { get; set; } = null!;
		[CascadingParameter] public Task<AuthenticationState> AuthStateTask { get; set; } = null!;
		protected AuthenticationState? AuthState { get; set; }

		private List<UsersDto>? AllUsers { get; set; }
		protected override async Task OnInitializedAsync()
		{
			Loading = true;
			StateHasChanged();
			AuthState = await AuthStateTask;
			if (AuthState is null) { Navigation.NavigateTo("/", true); return; }

			await GetAsync();
			if (AuthState.User.IsInRole("admin"))
			{
				await SearchUsers(null);
				if (!IsNew)
				{
					SelectedUser =
						$"({AllUsers?.FirstOrDefault(x=>x.Id==Entity.UserId)?.Email}) - {AllUsers?.FirstOrDefault(x=>x.Id==Entity.UserId)?.Name} {AllUsers?.FirstOrDefault(x => x.Id == Entity.UserId)?.SurName}";
				}
			}
			else
			{
				AllUsers = new List<UsersDto>()
				{
					new()
					{
						Id = AuthState.User.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Subject)?.Value,
						Email = AuthState.User.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Email)?.Value,
					}
				};
				SelectedUser =
					$"({AuthState.User.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Email)?.Value}) - {AuthState.User.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Name)?.Value}";
			}
			Loading = false;
			StateHasChanged();
		}

		protected string? SelectedUser { get; set; }

		protected bool IsAccountConnected { get; set; }
		private async Task SearchUsers(string? text, CancellationToken cancellationToken = default)
		{
			Loading = true;
			StateHasChanged();
			var userResponse = await UserService.GetAllByNameSurname(text, cancellationToken);
			if (!userResponse.Success)
			{
				Loading = false;
				StateHasChanged();
				Snackbar.Add(userResponse.Message, Severity.Error);
				return;
			}

			AllUsers = userResponse.Data;
			Loading = false;
			StateHasChanged();
		}
		protected async Task<IEnumerable<string>> SearchComboBox(string value)
		{
			if (string.IsNullOrEmpty(value) || value.Length < 3)
				return AllUsers is null ? new List<string>() : AllUsers.Select(x => new string($"({x.Email}) - {x.Name} {x.SurName}"));
			await SearchUsers(value);
			return AllUsers is null ? new List<string>() : AllUsers.Select(x => new string($"({x.Email}) - {x.Name} {x.SurName}"));
		}
		protected async Task ConnectTradingPlatform()
		{
			Loading = true;
			StateHasChanged();
			var balanceResponse = await TradingPlatformService.GetAccountBalance(Entity.ApiKey ?? "", Entity.SecretKey ?? "", Entity.Platform);
			if (!balanceResponse.Success)
			{
				IsAccountConnected = false;
				Loading = false;
				StateHasChanged();
				Snackbar.Add(balanceResponse.Message, Severity.Error);
				return;
			}

			Entity.BalanceSettings.CurrentBalance = balanceResponse.Data;
			IsAccountConnected = true;
			Loading = false;
			StateHasChanged();
		}
		protected void AdjustBalance()
		{
			if (!(Entity.BalanceSettings is { CurrentBalance: > 0, AdjustFrequencyDay: > 0, AdjustBalancePercentage: > 0 }))
			{
				Snackbar.Add(Localizer["TradingAccount_AdjustBalanceError"], Severity.Error);
				return;
			}

			Entity.BalanceSettings.CurrentAdjustedBalance =
				(Entity.BalanceSettings.CurrentBalance * Entity.BalanceSettings.AdjustBalancePercentage) / 100;
			Entity.BalanceSettings.LastAdjust = DateTime.Now;
		}

		protected async Task CheckAndSaveChangesAsync()
		{

			var user = AllUsers?.FirstOrDefault(x =>
				x.Email == SelectedUser?.Split('-')[0].Replace("(", "").Replace(")", "").Replace(" ", ""));
			if (user is null)
			{
				Snackbar.Add(Localizer["UserNotFound"], Severity.Error);
				return;
			}
			Entity.UserId=user.Id;
			await SaveChangesAsync();
		}
	}
}
