namespace TradingBot.Backend.Libraries.Domain.Defaults
{
	public static class Error
	{
		public static class NotFound
		{
			public const string RemoteApiEmptyResponse = "RemoteApiEmptyResponse";
		}
		public static class BadRequest
		{
		}
		public static class DbException
		{
			public const string Duplicate = "DuplicateRecord";
			public const string ReferenceConstraint = "TableRelated";
		}
	}
}
