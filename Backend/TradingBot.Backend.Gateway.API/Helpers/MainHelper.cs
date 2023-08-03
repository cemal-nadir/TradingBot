namespace TradingBot.Backend.Gateway.API.Helpers
{
	public static class MainHelper
	{
		public static string ToQueryString<T>(this T source,string? queryName)
		{
			var result = "";

			foreach (var property in typeof(T).GetProperties())
			{
				var name =queryName ?? property.Name;
				var value = property.GetValue(source, null);
				if (value != null)
					result += $"{(string.IsNullOrEmpty(result) ? "" : "&")}{name}={value}";
			}

			return result;
		}
		public static string ToQueryString(this List<string?>? source, List<string> userIds, string queryName)
		{
			if (source is null) return string.Empty;
			return source.Where(query => query != null).Aggregate("",
				(current, query) => current + $"{(string.IsNullOrEmpty(current) ? "" : "&")}{queryName}={query}");
		}
	}
}
