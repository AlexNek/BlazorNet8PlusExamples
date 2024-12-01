using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BlazorAuthentication.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            var services = builder.Services;
            services.AddHttpClient(nameof(ApiAuthenticationStateProviderExample), client =>
                {
                    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress); // Use the base address from host environment
                });

            services.AddAuthorizationCore();
            services.AddCascadingAuthenticationState();

            // Register the custom authentication state provider
            services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

            await builder.Build().RunAsync();
        }
    }
}
