using System.Net;

namespace TradingBot.Backend.Gateway.API.Responses;

public class SuccessResponse : Response
{
    public SuccessResponse() : base(true, "", HttpStatusCode.OK)
    {
    }

    public SuccessResponse(string message) : base(true, message, HttpStatusCode.OK)
    {
    }
}

public class SuccessResponse<T> : Response<T>
{
    public SuccessResponse(T? data) : base(true, "", HttpStatusCode.OK, data)
    {
    }

    public SuccessResponse(T? data, string message) : base(true, message, HttpStatusCode.OK, data)
    {
    }
}