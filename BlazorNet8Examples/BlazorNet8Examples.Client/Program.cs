using BlazorNet8RenderModes.Client.Services;
using BlazorNet8RenderModes.Common;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BlazorNet8RenderModes.Client
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            //builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IRenderModeService, RenderModeServiceClient>();
            builder.Services.AddSingleton<ICounterStateService, CounterStateClient>();

            await builder.Build().RunAsync();
        }
    }
}
