namespace TradingBot.Frontend.Libraries.Blazor.Defaults
{
	public static class ErrorDefaults
	{
		public static class NotFound
		{
			public const string RemoteApiEmptyResponse = "RemoteApiEmptyResponse";
			public const string UserRolesNotFound = "UserRolesNotFound";
			public const string UserInfoNotFound = "UserInfoNotFound";
			public const string TokenNotFound = "TokenNotFound";

		}
		public static class BadRequest
		{
			public const string UnAuthorized = "UnAuthorized";
		}
		public static class DbException
		{
			public const string Duplicate = "DuplicateRecord";
			public const string ReferenceConstraint = "TableRelated";
		}
		public static class Validation
		{
			public const string MaximumLength = "MaximumLengthError";
			public const string NotEmpty = "NotEmpty";
		}
	}
}
