using FluentValidation;

namespace TradingBot.Frontend.Libraries.Blazor.Services
{
	public class ValidatorBase<T>:AbstractValidator<T> where T:class
	{
		public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
		{
			var result = await ValidateAsync(ValidationContext<T>.CreateWithOptions((T)model, x => x.IncludeProperties(propertyName)));
			return result.IsValid ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage);
		};
	}
}
