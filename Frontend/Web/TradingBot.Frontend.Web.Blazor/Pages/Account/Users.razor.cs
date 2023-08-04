using TradingBot.Frontend.Web.Blazor.Components.Bases;
using TradingBot.Frontend.Web.Blazor.Dtos.Identity;
using TradingBot.Frontend.Web.Blazor.Services.Abstract.User;

namespace TradingBot.Frontend.Web.Blazor.Pages.Account
{
	public class UsersRazor:BaseListPage<UserDto,UsersDto,string,IUserService,User,UserValidator>
	{
	
		protected async Task AddNew()
		{
			await CreateAsync();
		}

	}
}
