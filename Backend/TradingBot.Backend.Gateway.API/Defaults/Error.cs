namespace TradingBot.Backend.Gateway.API.Defaults
{
	public class Error
	{
		public static class NotFound
		{
			public const string RemoteApiEmptyResponse = "RemoteApiEmptyResponse";
			public const string DataEmptyResponse = "DataEmptyResponse";
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
