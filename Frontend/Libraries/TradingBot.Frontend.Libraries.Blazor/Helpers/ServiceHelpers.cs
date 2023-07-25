using System.Web;
using CNG.Http.Responses;
using TradingBot.Frontend.Libraries.Blazor.Responses;

namespace TradingBot.Frontend.Libraries.Blazor.Helpers
{
	public static class ServiceHelpers
	{
		public static Response<T> GetResponse<T>(this HttpClientResponse<T> response)
		{
			return response.Success
				? new SuccessResponse<T>(response.Data)
				: new ErrorResponse<T>(response.Message, response.StatusCode);
		}
		public static Response GetResponse(this HttpClientResponse response)
		{
			return response.Success
				? new SuccessResponse()
				: new ErrorResponse(response.Message, response.StatusCode);
		}
		public static string ToUrlQueryString(this List<string> listOfId, string? queryName = null)
		{
			return string.Join("&", listOfId.Select(id => $"{queryName ?? nameof(listOfId)}={HttpUtility.UrlEncode(id)}"));
		}
	}
}
