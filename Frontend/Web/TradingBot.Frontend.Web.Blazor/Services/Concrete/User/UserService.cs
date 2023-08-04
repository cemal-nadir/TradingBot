using CNG.Core;
using CNG.Http.Services;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using TradingBot.Frontend.Libraries.Blazor.Defaults;
using TradingBot.Frontend.Libraries.Blazor.Models;
using TradingBot.Frontend.Libraries.Blazor.Repositories;
using TradingBot.Frontend.Libraries.Blazor.Responses;
using TradingBot.Frontend.Web.Blazor.Dtos.Identity;
using TradingBot.Frontend.Web.Blazor.Services.Abstract.User;

namespace TradingBot.Frontend.Web.Blazor.Services.Concrete.User
{
	public class UserService:ServiceRepository<string,UserDto,UsersDto>,IUserService
	{
		private readonly IHttpClientService _client;
		public UserService(IHttpClientService client, Env env, ProtectedLocalStorage protectedLocalStorage) : base(client, env.GatewayUrl.Substring(0,env.GatewayUrl.Length-1),  ServiceDefaults.User.UserService, protectedLocalStorage)
		{
			_client = client;
		}

		#region CRUD

		public async Task<Response<List<UsersDto>>> GetAllByNameSurname(string? searchText,CancellationToken cancellationToken = default)
		{
			_client.SetBearerAuthentication(await GetAccessToken());
			var response = await _client.GetAsync<List<UsersDto>>($"{BaseUrl}/{ServiceUrl}/Search?{nameof(searchText)}={searchText}", cancellationToken);
			if (!response.Success) return new ErrorResponse<List<UsersDto>>(response.Message, response.StatusCode);

			return new SuccessResponse<List<UsersDto>>(response.Data);

		}

		public async Task<Response<UserDto>> GetCurrentUser(CancellationToken cancellationToken = default)
		{
			_client.SetBearerAuthentication(await GetAccessToken());
			var response = await _client.GetAsync<UserDto>($"{BaseUrl}/{ServiceUrl}/CurrentUser", cancellationToken);
			if (!response.Success) return new ErrorResponse<UserDto>(response.Message, response.StatusCode);

			return new SuccessResponse<UserDto>(response.Data);
		}

		#endregion

		#region Roles


		public async Task<Response<List<SelectList<string>>>> GetRoles(CancellationToken cancellationToken = default)
		{
			_client.SetBearerAuthentication(await GetAccessToken());
			var response = await _client.GetAsync<List<SelectList<string>>>($"{BaseUrl}/{ServiceUrl}/Roles", cancellationToken);
			if (!response.Success) return new ErrorResponse<List<SelectList<string>>>(response.Message, response.StatusCode);

			return new SuccessResponse<List<SelectList<string>>>(response.Data);
		}


		public async Task<Response<List<SelectList<string>>>> GetUserRoles(string id, CancellationToken cancellationToken = default)
		{
			_client.SetBearerAuthentication(await GetAccessToken());
			var response = await _client.GetAsync<List<SelectList<string>>>($"{BaseUrl}/{ServiceUrl}/{id}/Roles", cancellationToken);
			if (!response.Success) return new ErrorResponse<List<SelectList<string>>>(response.Message, response.StatusCode);

			return new SuccessResponse<List<SelectList<string>>>(response.Data);
		}


		#endregion

		#region User Confirmation


		public async Task<Response<string>> GenerateUserConfirmToken(string id, CancellationToken cancellationToken = default)
		{
			_client.SetBearerAuthentication(await GetAccessToken());
			var response = await _client.PostAsync($"{BaseUrl}/{ServiceUrl}/{id}/GenerateUserConfirmationToken", string.Empty, cancellationToken);
			if (!response.Success) return new ErrorResponse<string>(response.Message, response.StatusCode);

			return new SuccessResponse<string>(response.Message);

		}


		public async Task<Response> ConfirmUserToken(string id, string token, CancellationToken cancellationToken = default)
		{
			_client.SetBearerAuthentication(await GetAccessToken());
			var response = await _client.PostAsync($"{BaseUrl}/{ServiceUrl}/{id}/ValidateUserConfirmation?token={token}", string.Empty, cancellationToken);
			if (!response.Success) return new ErrorResponse(response.Message, response.StatusCode);

			return new SuccessResponse();
		}

		#endregion

		#region Password Reset


		public async Task<Response<string>> GeneratePassWordResetToken(string id, CancellationToken cancellationToken = default)
		{
			_client.SetBearerAuthentication(await GetAccessToken());
			var response = await _client.PostAsync($"{BaseUrl}/{ServiceUrl}/{id}/GeneratePasswordResetToken", string.Empty, cancellationToken);
			if (!response.Success) return new ErrorResponse<string>(response.Message, response.StatusCode);

			return new SuccessResponse<string>(response.Message);

		}

		public async Task<Response> ResetPassword(string id, ResetPasswordDto dto, CancellationToken cancellationToken = default)
		{
			_client.SetBearerAuthentication(await GetAccessToken());
			var response = await _client.PostAsync($"{BaseUrl}/{ServiceUrl}/{id}/ResetPassword", dto, cancellationToken);
			if (!response.Success) return new ErrorResponse(response.Message, response.StatusCode);

			return new SuccessResponse();

		}


		public async Task<Response> ChangePassword(ChangePasswordDto dto, CancellationToken cancellationToken = default)
		{
			_client.SetBearerAuthentication(await GetAccessToken());
			var response = await _client.PostAsync($"{BaseUrl}/{ServiceUrl}/ChangePassword", dto, cancellationToken);
			if (!response.Success) return new ErrorResponse(response.Message, response.StatusCode);

			return new SuccessResponse();
		}

		#endregion
	}
}
