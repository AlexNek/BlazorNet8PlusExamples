using System.Text;
using System.Text.Json;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorAuthentication.Client.Pages
{
    public partial class ClientLogoutPage : ComponentBase
    {
        [SupplyParameterFromQuery(Name = "origin")]
        private string? Origin { get; set; }

        [SupplyParameterFromQuery(Name = "alertId")]
        private string? AlertIdStr { get; set; }

        [Inject]
        NavigationManager Navigation { get; set; }

        [Inject]
        private IHttpClientFactory HttpClientFactory { get; set; }
        
        [Inject]
        private AntiforgeryStateProvider AntiforgeryStateProvider { get; set; }
        private async Task LogoutHandler()
        {
            try
            {
                var client = HttpClientFactory.CreateClient(nameof(ApiAuthenticationStateProviderExample));

                var tokenResult =  AntiforgeryStateProvider.GetAntiforgeryToken();

                if (tokenResult != null)
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, "/logout-http");
                    request.Headers.Add(tokenResult.FormFieldName, tokenResult.Value);

                    var content = new FormUrlEncodedContent(new[]
                                                                {
                                                                    new KeyValuePair<string, string>("returnUrl", $"/{Origin}")
                                                                });
                    request.Content = content;

                    var response = await client.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        // Handle successful logout
                        // For example:
                         Navigation.NavigateTo($"/{Origin}", forceLoad: true);
                    }
                    else
                    {
                        // Handle error
                        var errorContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Error: {response.StatusCode}, Content: {errorContent}");
                    }
                }
                else
                {
                    Console.WriteLine("Failed to get antiforgery token");
                }


                //var request = new HttpRequestMessage(HttpMethod.Post, "/logout-http");
                //var response = await client.SendAsync(request);

                //var content = new StringContent(
                //    JsonSerializer.Serialize(new { returnUrl = "/" }),
                //    Encoding.UTF8,
                //    "application/json");

                //var response = await client.PostAsync("/logout-http", content);
                //await Http.GetAsync("/logout-http");
                //await JSRuntime.InvokeVoidAsync("window.location.href", url);

                //HttpContext.RedirectTo(url);

                //Navigation.NavigateTo(url, forceLoad: true);
            }
            catch (Exception ex)
            {
                int breakpointPlaceholder = 0;
                //Logger.LogError(ex, "Error during logout process");
            }
        }
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LogoutHandler();
        }
    }
}
