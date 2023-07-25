using System.IdentityModel.Tokens.Jwt;
using Amazon.Runtime.Internal.Transform;
using CNG.Abstractions.Signatures;
using CNG.Http.Services;
using TradingBot.Backend.Gateway.API.Responses;
using TradingBot.Backend.Gateway.API.Services.Abstract.Token;

namespace TradingBot.Backend.Gateway.API.Repositories
{
	public abstract class ServiceRepository<TKey, TDto, TListDto> : IServiceRepository<TKey, TDto, TListDto>
		where TDto : IDto, new()
		where TListDto : IListDto<TKey?>, new()
		where TKey : IEquatable<TKey?>
	{
		private readonly IHttpClientService _client;
		private readonly string _url;
		protected ServiceRepository(IHttpClientService client, string url, string clientName, IHttpContextAccessor httpContextAccessor, ITokenService tokenService)
		{
			_client = client;
			_client.SetClient(clientName);
			_url = url;
			_client.SetBearerAuthentication(tokenService.GetClientCredentialToken().Result);
			_client.SetHeader(new Dictionary<string, string>()
			{
				new("X-User",httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x=>x.Type==JwtRegisteredClaimNames.Sub)?.Value??"") 
			});
		}
		public virtual async Task<Response<List<TListDto>>> GetAllAsync(CancellationToken cancellationToken = default)
		{
			var response = await _client.GetAsync<List<TListDto>>(_url, cancellationToken);
			return response.Success
				? new SuccessResponse<List<TListDto>>(response.Data)
				: new ErrorResponse<List<TListDto>>(response.Message, response.StatusCode);
		}

		public virtual async Task<Response<TDto>> GetAsync(TKey id, CancellationToken cancellationToken = default)
		{
			var response = await _client.GetAsync<TDto>($"{_url}/{id}", cancellationToken);
			return response.Success
				? new SuccessResponse<TDto>(response.Data)
				: new ErrorResponse<TDto>(response.Message, response.StatusCode);
		}

		public virtual async Task<Response> InsertAsync(TDto dto, CancellationToken cancellationToken = default)
		{
			var response = await _client.PostAsync($"{_url}", dto, cancellationToken);
			return response.Success
				? new SuccessResponse()
				: new ErrorResponse(response.Message, response.StatusCode);
		}

		public virtual async Task<Response> UpdateAsync(TKey id, TDto dto, CancellationToken cancellationToken = default)
		{
			var response = await _client.HttpPutAsync($"{_url}/{id}", dto, cancellationToken);
			return response.Success
				? new SuccessResponse()
				: new ErrorResponse(response.Message, response.StatusCode);
		}

		public virtual async Task<Response> DeleteAsync(TKey id, CancellationToken cancellationToken = default)
		{
			var response = await _client.DeleteAsync($"{_url}/{id}", cancellationToken);
			return response.Success
				? new SuccessResponse()
				: new ErrorResponse(response.Message, response.StatusCode);
		}

		public virtual async Task<Response> DeleteRangeAsync(IEnumerable<TKey> listOfId, CancellationToken cancellationToken = default)
		{
			var query = listOfId.Aggregate("", (current, id) => current + $"{(string.IsNullOrEmpty(current) ? "" : "&")}listOfId={id}");
			var response = await _client.DeleteAsync($"{_url}?{query}", cancellationToken);
			return response.Success
				? new SuccessResponse()
				: new ErrorResponse(response.Message, response.StatusCode);
		}

		public virtual async Task<Response> RemoveCacheAsync(CancellationToken cancellationToken = default)
		{
			var response = await _client.PostAsync($"{_url}/RemoveCache", cancellationToken);
			return response.Success
				? new SuccessResponse()
				: new ErrorResponse(response.Message, response.StatusCode);
		}
	}
}
