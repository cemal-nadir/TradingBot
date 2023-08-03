using Microsoft.AspNetCore.Components;
using TradingBot.Frontend.Web.Blazor.Components.Bases;
using TradingBot.Frontend.Web.Blazor.Dtos.Users;
using TradingBot.Frontend.Web.Blazor.Services.Abstract.User;

namespace TradingBot.Frontend.Web.Blazor.Pages.TradingAccounts
{
	public class TradingAccountsRazor : BaseListPage<TradingAccountDto, TradingAccountsDto, string, ITradingAccountService, TradingAccount, TradingAccountValidator>
	{
		[Parameter] public string? UserId { get; set; }
		protected override async Task GetAllAsync(CancellationToken cancellationToken = default)
		{
			Loading = true;
			StateHasChanged();
			if (UserId is null)
			{
				var response = await Service.GetAllAsync(cancellationToken);
				Entities = response.Data;

			}
			else
			{
				var response = await Service.GetTradingAccountsByUserId(UserId,cancellationToken);
				Entities = response.Data;
			}
			Loading = false;
		}
		protected async Task AddNew()
		{
			await CreateAsync();
		}
	}
}
