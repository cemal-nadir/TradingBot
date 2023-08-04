using System.Net;
using IdentityModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using TradingBot.Frontend.Web.Blazor.Components.Bases;
using TradingBot.Frontend.Web.Blazor.Dtos.Users;
using TradingBot.Frontend.Web.Blazor.Services.Abstract.User;

namespace TradingBot.Frontend.Web.Blazor.Pages.TradingAccounts
{
	public class TradingAccountsRazor : BaseListPage<TradingAccountDto, TradingAccountsDto, string, ITradingAccountService, TradingAccount, TradingAccountValidator>
	{
		[Parameter] public string? UserId { get; set; }
		[Inject] private IUserService UserService { get; set; } = null!;
		[CascadingParameter] public Task<AuthenticationState> AuthStateTask { get; set; } = null!;

		protected override async Task GetAllAsync(CancellationToken cancellationToken = default)
		{
			Loading = true;
			StateHasChanged();
			var authState = await AuthStateTask;
			if (UserId is null)
			{
				if (authState.User.IsInRole("admin"))
				{
					Entities = (await Service.GetAllAsync(cancellationToken)).Data;
				}
				else
				{
					Entities = (await Service.GetTradingAccountsByUserId(authState.User.Claims
						.FirstOrDefault(x => x.Type == JwtClaimTypes.Subject)?.Value ?? "", cancellationToken)).Data;
				}

			}
			else
			{
				if (authState.User.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Subject)?.Value != UserId)
					Navigation.NavigateTo("/TradingAccounts", true);

				var user = await UserService.GetAsync(UserId, cancellationToken);
				if (!user.Success)
				{
					Snackbar.Add(user.Message, Severity.Error);
					StateHasChanged();
					await Task.Delay(2000, cancellationToken);
				}
				if (user.StatusCode is HttpStatusCode.NotFound)
				{
					Navigation.NavigateTo("/TradingAccounts", true);
				}
				var response = await Service.GetTradingAccountsByUserId(UserId, cancellationToken);
				Entities = response.Data;
			}
			Loading = false;
			StateHasChanged();
		}
		protected async Task AddNew()
		{
			await CreateAsync();
		}
	}
}
