using System.Runtime.InteropServices;

using BlazorNet8RenderModes.Common;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BlazorNet8RenderModes.Client.Services;

public class RenderModeServiceClient : IRenderModeService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly NavigationManager _navigationManager;

    public RenderModeServiceClient(IServiceProvider serviceProvider, NavigationManager navigationManager)
    {
        _serviceProvider = serviceProvider;
        _navigationManager = navigationManager;
    }

    public string GetRenderMode()
    {
        // Check for WebAssembly
        if (_serviceProvider.GetService(typeof(IWebAssemblyHostEnvironment)) != null)
        {
            return "WebAssembly";
        }
        if (RuntimeInformation.ProcessArchitecture != Architecture.Wasm)
        {
            //renderMode = "server";  //the architecture could be x64 depending on your machine.
        }
        else
        {
            //renderMode = "wasm";
        }

        return "Unknown";
    }

    
}