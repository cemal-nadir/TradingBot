namespace TradingBot.Backend.Services.Identity.Api.Middlewares;

public static class ErrorDefaults
{
    public static class NotFound
    {
        public const string User = "UserNotFound";
        public const string Role = "RoleNotFound";
    }
    public static class BadRequest
    {
        public const string Password = "WrongPassword";
    }
    public static class DbException
    {
        public const string Duplicate = "DuplicateRecord";
        public const string ReferenceConstraint = "TableRelated";
    }
}