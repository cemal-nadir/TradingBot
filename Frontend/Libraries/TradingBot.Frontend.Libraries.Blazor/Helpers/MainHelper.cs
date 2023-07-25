using System.Globalization;
using System.Text.RegularExpressions;

namespace TradingBot.Frontend.Libraries.Blazor.Helpers
{
    public static class MainHelper
    {
        public static string GetEnumValueCustom<T>(this T enumProperty) where T : Enum => Regex.Replace(enumProperty.ToString(), @"(?<=[a-z])([A-Z])", @"_$1").ToLower(CultureInfo.CreateSpecificCulture("en"));
        public static T? ConvertStringToEnumValueCustom<T>(this string stringValue) where T : struct, Enum
        {
            var valueArr = stringValue.Split('_');
            var value = valueArr.Aggregate(string.Empty, (current, variableNameItem) => current + variableNameItem switch
            {
                null => throw new ArgumentNullException(),
                "" => throw new ArgumentException(""),
                _ => string.Concat(variableNameItem[0].ToString().ToUpper(CultureInfo.CreateSpecificCulture("en")), variableNameItem.AsSpan(1))
            });
            if (Enum.TryParse<T>(value, true, out var result)) return result;

            return null;

        }

        public static string ToQueryString<T>(this T source)
        {
	        var result = "";

	        foreach (var property in typeof(T).GetProperties())
	        {
		        var name = property.Name;
		        var value = property.GetValue(source, null);
		        if (value != null)
			        result += $"{(string.IsNullOrEmpty(result) ? "" : "&")}{name}={value}";
	        }

	        return result;
        }
    }
}
