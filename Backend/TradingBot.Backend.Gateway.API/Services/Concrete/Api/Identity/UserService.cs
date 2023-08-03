using CNG.Http.Services;
using CNG.Core;
using TradingBot.Backend.Gateway.API.Defaults;
using TradingBot.Backend.Gateway.API.Dtos;
using TradingBot.Backend.Gateway.API.Dtos.Requests.Identity;
using TradingBot.Backend.Gateway.API.Helpers;
using TradingBot.Backend.Gateway.API.Responses;
using TradingBot.Backend.Gateway.API.Services.Abstract.Api.Identity;
using TradingBot.Backend.Gateway.API.Services.Abstract.Token;

namespace TradingBot.Backend.Gateway.API.Services.Concrete.Api.Identity
{
	public class UserService:IUserService
	{
		private readonly IHttpClientService _client;
		private readonly string _url;

		public UserService(IHttpClientService client,EnvironmentModel env, ITokenService tokenService)
		{
			_client = client;
			_client.SetClient(Client.ResourceOwnerPasswordClient);
			_url = $"{env.MicroServices?.Identity}/api/{Defaults.Gateway.User.UserService}";
			_client.SetBearerAuthentication(tokenService.GetResourceOwnerPasswordToken());
		}

		#region CRUD
		
		
		public async Task<Response<List<UsersDto>>> GetAllUsers(CancellationToken cancellationToken=default)
		{
			var response=await _client.GetAsync<List<UsersDto>>($"{_url}", cancellationToken);
			if (!response.Success) return new ErrorResponse<List<UsersDto>>(response.Message, response.StatusCode);

			return new SuccessResponse<List<UsersDto>>(response.Data);

		}
		public async Task<Response<List<UsersDto>>> GetAllByUserIdsAsync(List<string> listOfId, CancellationToken cancellationToken = default)
		{
			
			var response =
				await _client.GetAsync<List<UsersDto>>(
					$"{_url}?{listOfId!.ToQueryString(listOfId, nameof(listOfId))}", cancellationToken);
			return response.Success
				? new SuccessResponse<List<UsersDto>>(response.Data)
				: new ErrorResponse<List<UsersDto>>(response.Message, response.StatusCode);
		}

		public async Task<Response<UserDto>> GetUser(string id,CancellationToken cancellationToken=default)
		{
			var response = await _client.GetAsync<UserDto>($"{_url}/{id}", cancellationToken);
			if (!response.Success) return new ErrorResponse<UserDto>(response.Message, response.StatusCode);

			return new SuccessResponse<UserDto>(response.Data);
		}
		
		
		public async Task<Response> InsertUser(UserInsertDto dto,CancellationToken cancellationToken=default)
		{
			var response = await _client.PostAsync($"{_url}", dto,cancellationToken);
			if (!response.Success) return new ErrorResponse(response.Message, response.StatusCode);

			return new SuccessResponse();
		}
		
		
		public async Task<Response> UpdateUser(UserUpdateDto dto,CancellationToken cancellationToken=default)
		{
			var response = await _client.HttpPutAsync($"{_url}", dto, cancellationToken);
			if (!response.Success) return new ErrorResponse(response.Message, response.StatusCode);

			return new SuccessResponse();
		}
		
		
		public async Task<Response> DeleteUser(string id,CancellationToken cancellationToken=default)
		{
			var response = await _client.DeleteAsync($"{_url}/{id}", cancellationToken);
			if (!response.Success) return new ErrorResponse(response.Message, response.StatusCode);

			return new SuccessResponse();
		}
		
		
		public async Task<Response<UserDto>> GetCurrentUser(CancellationToken cancellationToken=default)
		{
			var response = await _client.GetAsync<UserDto>($"{_url}/CurrentUser", cancellationToken);
			if (!response.Success) return new ErrorResponse<UserDto>(response.Message, response.StatusCode);

			return new SuccessResponse<UserDto>(response.Data);
		}

		#endregion

		#region Roles
		
		
		public async Task<Response<List<SelectList<string>>>> GetRoles(CancellationToken cancellationToken=default)
		{
			var response = await _client.GetAsync<List<SelectList<string>>>($"{_url}/Roles", cancellationToken);
			if (!response.Success) return new ErrorResponse<List<SelectList<string>>>(response.Message, response.StatusCode);

			return new SuccessResponse<List<SelectList<string>>>(response.Data);
		}
		
	
		public async Task<Response<List<SelectList<string>>>> GetUserRoles(string id,CancellationToken cancellationToken=default)
		{
			var response = await _client.GetAsync<List<SelectList<string>>>($"{_url}/{id}/Roles", cancellationToken);
			if (!response.Success) return new ErrorResponse<List<SelectList<string>>>(response.Message, response.StatusCode);

			return new SuccessResponse<List<SelectList<string>>>(response.Data);
		}


		#endregion

		#region User Confirmation
	
	
		public async Task<Response<string>> GenerateUserConfirmToken(string id,CancellationToken cancellationToken=default)
		{
			var response = await _client.PostAsync($"{_url}/{id}/GenerateUserConfirmationToken",string.Empty, cancellationToken);
			if (!response.Success) return new ErrorResponse<string>(response.Message, response.StatusCode);

			return new SuccessResponse<string>(response.Message);

		}
	
	
		public async Task<Response> ConfirmUserToken(string id,string token,CancellationToken cancellationToken=default)
		{
			var response = await _client.PostAsync($"{_url}/{id}/ValidateUserConfirmation?token={token}", string.Empty, cancellationToken);
			if (!response.Success) return new ErrorResponse(response.Message, response.StatusCode);

			return new SuccessResponse();
		}

		#endregion

		#region Password Reset
		
	
		public async Task<Response<string>> GeneratePassWordResetToken(string id,CancellationToken cancellationToken=default)
		{
			var response = await _client.PostAsync($"{_url}/{id}/GeneratePasswordResetToken", string.Empty, cancellationToken);
			if (!response.Success) return new ErrorResponse<string>(response.Message, response.StatusCode);

			return new SuccessResponse<string>(response.Message);
			
		}

		public async Task<Response> ResetPassword(string id, ResetPasswordDto dto,CancellationToken cancellationToken=default)
		{
			var response = await _client.PostAsync($"{_url}/{id}/ResetPassword", dto, cancellationToken);
			if (!response.Success) return new ErrorResponse(response.Message, response.StatusCode);

			return new SuccessResponse();
			
		}
	
	
		public async Task<Response> ChangePassword(ChangePasswordDto dto,CancellationToken cancellationToken=default)
		{
			var response = await _client.PostAsync($"{_url}/ChangePassword", dto, cancellationToken);
			if (!response.Success) return new ErrorResponse(response.Message, response.StatusCode);

			return new SuccessResponse();
		}

		#endregion
	}
}
