using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace BlazorAuthentication.Components.Account.Pages
{
    public partial class ServerLogoutPage : ComponentBase
    {
        [CascadingParameter]
        private HttpContext HttpContext { get; set; } = default!;

        [SupplyParameterFromQuery(Name = "origin")]
        private string? Origin { get; set; }

        [SupplyParameterFromQuery(Name = "alertId")]
        private string? AlertIdStr { get; set; }

        [Inject]
        NavigationManager Navigation { get; set; }

        private async Task LogoutHandler()
        {
            // Sign out the user by clearing the authentication cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Ensure Origin is not null or empty
            if (string.IsNullOrWhiteSpace(Origin))
            {
                Origin = ""; // Set a default path if Origin is not provided
            }

            // Construct the final URL, including AlertIdStr if available
            var queryParams = new Dictionary<string, string?>();

            if (!string.IsNullOrWhiteSpace(AlertIdStr))
            {
                queryParams["alertId"] = AlertIdStr;
            }

            var finalUri = QueryHelpers.AddQueryString($"/{Origin}", queryParams);

            // Redirect to the constructed URL
            // exception as we must use SSR render mode
            Navigation.NavigateTo(finalUri, true);
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LogoutHandler();
        }
    }
}
