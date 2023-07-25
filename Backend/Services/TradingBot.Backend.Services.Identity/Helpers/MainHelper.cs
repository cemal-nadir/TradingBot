using Newtonsoft.Json;

namespace TradingBot.Backend.Services.Identity.Api.Helpers;

public static class MainHelper
{
    public static bool IsEqual<TModel, TCompareModel>(TModel model, TCompareModel compareModel)
        where TModel : class where TCompareModel : class
    {
        return JsonConvert.SerializeObject(model) == JsonConvert.SerializeObject(compareModel);
    }
}