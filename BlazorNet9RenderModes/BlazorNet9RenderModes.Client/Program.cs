using BlazorNet9RenderModes.Client.Services;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BlazorNet9RenderModes.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.Services.AddSingleton<ICounterStateService, CounterStateClient>();
            await builder.Build().RunAsync();
        }
    }
}
