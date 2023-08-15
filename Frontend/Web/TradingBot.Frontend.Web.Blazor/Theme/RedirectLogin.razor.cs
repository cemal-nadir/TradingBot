using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace TradingBot.Frontend.Web.Blazor.Theme;

public class RedirectLoginRazor:ComponentBase
{
    [CascadingParameter] public Task<AuthenticationState>? AuthState { get; set; }

    [Inject] public NavigationManager? NavigationManager { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (AuthState != null)
        {
            var authState = await AuthState;
            if (authState.User.Identity is null || !authState.User.Identity.IsAuthenticated)
            {
                var returnUrl = NavigationManager?.ToBaseRelativePath(NavigationManager.Uri);
                NavigationManager?.NavigateTo(string.IsNullOrEmpty(returnUrl) ? "login" : $"login?returnUrl={returnUrl}",true);
            }
        }
    }
}