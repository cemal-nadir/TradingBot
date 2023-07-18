using CNG.Core.Exceptions;
using CryptoExchange.Net.Objects;
using System.Net;

namespace TradingBot.Backend.Libraries.Infrastructure.Extensions
{
	public static class BinanceRestClientExtension
	{
		public static T CheckError<T>(this WebCallResult<T> result)
		{
			if (result.Success) return result.Data;
			if (result.ResponseStatusCode is HttpStatusCode.NotFound)
				throw new NotFoundException(result.Error?.Message ?? Domain.Defaults.Error.NotFound.RemoteApiEmptyResponse);
			throw new BadRequestException(result.Error?.Message ?? Domain.Defaults.Error.BadRequest.RemoteApiErrorResponse);
		}
	}
}
