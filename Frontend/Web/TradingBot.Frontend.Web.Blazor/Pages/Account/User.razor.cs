using CNG.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using TradingBot.Frontend.Libraries.Blazor.Signatures;
using TradingBot.Frontend.Web.Blazor.Components.Bases;
using TradingBot.Frontend.Web.Blazor.Dtos.Identity;
using TradingBot.Frontend.Web.Blazor.Services.Abstract.User;

namespace TradingBot.Frontend.Web.Blazor.Pages.Account
{
	public class UserRazor : BaseDetailPage<UserDto, UsersDto, string, IUserService, UserValidator>
	{
		protected bool ShowPassword { get; set; }
		protected List<SelectList<string>>? AllRoles { get; set; }

		protected string? SelectedRole { get; set; }

		protected async Task CheckAndSaveChangesAsync()
		{
			if (SelectedRole is null)
			{
				Snackbar.Add(Localizer["RoleNotFound"], Severity.Error);
				return;
			}

			Entity.Roles ??= new()
			{
				SelectedRole
			};
			Entity.UserName = Entity.Email;
			await SaveChangesAsync();
		}
		protected override async Task OnInitializedAsync()
		{

			await GetAsync();
			await GetAllRoles();
			StateHasChanged();


		}
		protected override async Task GetAsync(CancellationToken cancellationToken = default)
		{
			if (!IsNew)
			{
				if (Id != null)
				{
					Processing = true;
					var response = await Service.GetAsync(Id, cancellationToken);
					Processing = false;
					if (response.Success)
					{
						Entity = response.Data ?? new UserDto();
						SelectedRole = Entity.Roles?.FirstOrDefault();
					}
					else Snackbar.Add(response.Message, Severity.Error);
				}
			}
		}
		private async Task GetAllRoles()
		{
			Processing = true;
			StateHasChanged();
			AllRoles = (await Service.GetRoles()).Data;
			Processing = false;
			StateHasChanged();
		}
	}
}
