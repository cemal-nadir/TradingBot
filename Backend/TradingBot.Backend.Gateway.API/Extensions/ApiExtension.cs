using CNG.Core.Exceptions;
using System.Net;
using System.Security.Authentication;
using System.Security;
using CNG.Abstractions.Signatures;
using TradingBot.Backend.Gateway.API.Defaults;
using TradingBot.Backend.Gateway.API.Responses;

namespace TradingBot.Backend.Gateway.API.Extensions
{
	public static class ApiExtension
	{
		public static void CheckResponse(this Response response)
		{
			if (!response.Success)
				throw response.StatusCode switch
				{
					HttpStatusCode.BadRequest => new BadRequestException(response.Message),
					HttpStatusCode.NotFound => new NotFoundException(response.Message),
					HttpStatusCode.Unauthorized => new AuthenticationException(response.Message),
					HttpStatusCode.Forbidden => new SecurityException(response.Message),
					HttpStatusCode.InternalServerError => new Exception(response.Message),
					_ => new Exception(response.Message)
				};
		}
		public static T CheckResponse<T>(this Response<T> response) where T : class
		{
			if (!response.Success)
				throw response.StatusCode switch
				{
					HttpStatusCode.BadRequest => new BadRequestException(response.Message),
					HttpStatusCode.NotFound => new NotFoundException(response.Message),
					HttpStatusCode.Unauthorized => new AuthenticationException(response.Message),
					HttpStatusCode.Forbidden => new SecurityException(response.Message),
					HttpStatusCode.InternalServerError => new Exception(response.Message),
					_ => new Exception(response.Message)
				};

			if (response.Data is null) throw new NotFoundException(Error.NotFound.DataEmptyResponse);
			return response.Data;
		}
		public static List<T>? CheckResponse<T>(this Response<List<T>> response) where T : class
		{
			if (!response.Success)
				throw response.StatusCode switch
				{
					HttpStatusCode.BadRequest => new BadRequestException(response.Message),
					HttpStatusCode.NotFound => new NotFoundException(response.Message),
					HttpStatusCode.Unauthorized => new AuthenticationException(response.Message),
					HttpStatusCode.Forbidden => new SecurityException(response.Message),
					HttpStatusCode.InternalServerError => new Exception(response.Message),
					_ => new Exception(response.Message)
				};

			return response.Data;
		}

	}
}
