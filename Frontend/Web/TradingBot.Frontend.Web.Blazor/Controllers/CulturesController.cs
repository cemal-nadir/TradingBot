using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace TradingBot.Frontend.Web.Blazor.Controllers;

[Route("/api/v1/auth")]
public class CulturesController : Controller
{
    [HttpGet("setCulture")]
    public IActionResult SetCulture([FromQuery] string culture, [FromQuery] string redirectionUri)
    {
        if (!string.IsNullOrEmpty(culture))
        {
            HttpContext.Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(
                    new RequestCulture(culture)));
        }
        return LocalRedirect(redirectionUri);
    }
}