using System.Net;
using System.Security;
using System.Security.Authentication;
using CNG.Core.Exceptions;
using CNG.Http.Responses;
using TradingBot.Frontend.Libraries.Blazor.Defaults;
using TradingBot.Frontend.Libraries.Blazor.Services;

namespace TradingBot.Frontend.Web.Blazor.Handlers
{
	public class ErrorHandlingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly GlobalRenderService _globalRenderService;
		public ErrorHandlingMiddleware(RequestDelegate next, GlobalRenderService globalRenderService)
		{
			_next = next;
			_globalRenderService = globalRenderService;
		}

		public async Task Invoke(HttpContext context,IWebHostEnvironment env)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(ex,env);
			}
		}
		private Task HandleExceptionAsync(Exception exception, IWebHostEnvironment env)
		{
			HttpStatusCode status;
			string message;
			var stackTrace = string.Empty;

			var exceptionType = exception.GetType();
			if (exceptionType == typeof(BadRequestException))
			{
				message = exception.InnerException?.Message ?? exception.Message;
				status = HttpStatusCode.BadRequest;
			}
			else if (exceptionType == typeof(DbException))
			{
				message = exception.InnerException?.Message ?? exception.Message;
				status = HttpStatusCode.BadRequest;
			}
			else if (exceptionType == typeof(ValidationException))
			{
				message = exception.InnerException?.Message ?? exception.Message;
				status = HttpStatusCode.BadRequest;
			}
			else if (exceptionType == typeof(NotFoundException))
			{
				message = exception.InnerException?.Message ?? exception.Message;
				status = HttpStatusCode.NotFound;
			}
			else if (exceptionType == typeof(DbNullException))
			{
				message = exception.InnerException?.Message ?? exception.Message;
				status = HttpStatusCode.NotFound;
			}
			else if (exceptionType == typeof(AuthenticationException))
			{
				message = exception.InnerException?.Message ?? exception.Message;
				status = HttpStatusCode.Unauthorized;
			}
			else if (exceptionType == typeof(SecurityException))
			{
				message = exception.InnerException?.Message ?? exception.Message;
				status = HttpStatusCode.Forbidden;
			}
			else
			{
				status = HttpStatusCode.InternalServerError;
				message = exception.InnerException?.Message ?? exception.Message;
				if (env.IsEnvironment("Development"))
					stackTrace = exception.StackTrace;
			}

			RenderException(new ExceptionResponse()
			{
				StatusCode = (int)status,
				ExceptionType = exceptionType.Name,
				Message = ParseToMessage(message),
				StackTrace = stackTrace
			});
			return Task.CompletedTask;
		}
		private static string ParseToMessage(string message)
		{
			var result = message;
			if (message.Contains("insert duplicate key"))
			{
				result = ErrorDefaults.DbException.Duplicate;
			}
			else if (message.Contains("REFERENCE constraint \"FK_"))
			{
				var firstIndex = message.IndexOf("REFERENCE constraint \"FK_", StringComparison.Ordinal);
				var first = message.Substring(firstIndex, message.Length - firstIndex).Replace("REFERENCE constraint \"FK_", "").Split("_")[0];
				return $"{ErrorDefaults.DbException.ReferenceConstraint}|{first}";
			}
			return result;
		}
		public void RenderException(ExceptionResponse exceptionResponse)
		{
			_globalRenderService.RenderException(exceptionResponse);
		}
	}
}
