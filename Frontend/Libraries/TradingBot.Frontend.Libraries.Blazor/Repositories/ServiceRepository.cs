using CNG.Http.Services;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using TradingBot.Frontend.Libraries.Blazor.Defaults;
using TradingBot.Frontend.Libraries.Blazor.Models;
using TradingBot.Frontend.Libraries.Blazor.Responses;
using TradingBot.Frontend.Libraries.Blazor.Signatures;

namespace TradingBot.Frontend.Libraries.Blazor.Repositories;

public abstract class ServiceRepository<TKey, TDto, TListDto> : IServiceRepository<TKey, TDto, TListDto>
	where TDto : IDto, new()
	where TListDto : IListDto<TKey>, new()
	where TKey : IEquatable<TKey>
{
	private readonly IHttpClientService _client;
	protected readonly string Url;
	private readonly ProtectedLocalStorage _protectedLocalStorage;
	protected ServiceRepository(IHttpClientService client, string url, ProtectedLocalStorage protectedLocalStorage)
	{
		Url = url;
		_client = client;
		_client.SetClient(ClientDefaults.DefaultClient);
		_protectedLocalStorage = protectedLocalStorage;
	}

	public virtual async Task<string> GetAccessToken()
	{
		var result =
			await _protectedLocalStorage.GetAsync<AuthenticationTokenWithClaims>(nameof(AuthenticationTokenWithClaims));
		return result.Value?.AccessToken??"";

	}
	public virtual async Task<Response<List<TListDto>>> GetAllAsync(CancellationToken cancellationToken = default)
	{
		_client.SetBearerAuthentication(await GetAccessToken());
		var response = await _client.GetAsync<List<TListDto>>(Url, cancellationToken);
		return response.Success
			? new SuccessResponse<List<TListDto>>(response.Data)
			: new ErrorResponse<List<TListDto>>(response.Message, response.StatusCode);
	}

	public virtual async Task<Response<TDto>> GetAsync(TKey id, CancellationToken cancellationToken = default)
	{
		_client.SetBearerAuthentication(await GetAccessToken());
		var response = await _client.GetAsync<TDto>($"{Url}/{id}", cancellationToken);
		return response.Success
			? new SuccessResponse<TDto>(response.Data)
			: new ErrorResponse<TDto>(response.Message, response.StatusCode);
	}

	public virtual async Task<Response> InsertAsync(TDto dto, CancellationToken cancellationToken = default)
	{
		_client.SetBearerAuthentication(await GetAccessToken());
		var response = await _client.PostAsync($"{Url}", dto, cancellationToken);
		return response.Success
			? new SuccessResponse()
			: new ErrorResponse(response.Message, response.StatusCode);
	}

	public virtual async Task<Response> UpdateAsync(TKey id, TDto dto, CancellationToken cancellationToken = default)
	{
		_client.SetBearerAuthentication(await GetAccessToken());
		var response = await _client.HttpPutAsync($"{Url}/{id}", dto, cancellationToken);
		return response.Success
			? new SuccessResponse()
			: new ErrorResponse(response.Message, response.StatusCode);
	}

	public virtual async Task<Response> DeleteAsync(TKey id, CancellationToken cancellationToken = default)
	{
		_client.SetBearerAuthentication(await GetAccessToken());
		var response = await _client.DeleteAsync($"{Url}/{id}", cancellationToken);
		return response.Success
			? new SuccessResponse()
			: new ErrorResponse(response.Message, response.StatusCode);
	}

	public virtual async Task<Response> DeleteRangeAsync(IEnumerable<TKey> listOfId, CancellationToken cancellationToken = default)
	{
		_client.SetBearerAuthentication(await GetAccessToken());
		var query = listOfId.Aggregate("", (current, id) => current + $"{(string.IsNullOrEmpty(current) ? "" : "&")}listOfId={id}");
		var response = await _client.DeleteAsync($"{Url}?{query}", cancellationToken);
		return response.Success
			? new SuccessResponse()
			: new ErrorResponse(response.Message, response.StatusCode);
	}

	public virtual async Task<Response> RemoveCacheAsync(CancellationToken cancellationToken = default)
	{
		_client.SetBearerAuthentication(await GetAccessToken());
		var response = await _client.PostAsync($"{Url}/RemoveCache", cancellationToken);
		return response.Success
			? new SuccessResponse()
			: new ErrorResponse(response.Message, response.StatusCode);
	}
}