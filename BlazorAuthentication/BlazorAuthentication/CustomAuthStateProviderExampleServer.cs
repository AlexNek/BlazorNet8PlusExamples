using BlazorAuthentication.Client;

using Microsoft.AspNetCore.Authentication.Cookies;

namespace BlazorAuthentication;

public class CustomAuthStateProviderExampleServer : ApiAuthenticationStateProviderExample
{
    public CustomAuthStateProviderExampleServer(IHttpClientFactory httpClientFactory)
        : base(httpClientFactory)
    {
    }

    protected override string GetAuthenticationType()
    {
        //return "cookie"; // Server-side authentication type
        return CookieAuthenticationDefaults.AuthenticationScheme;
    }
}
