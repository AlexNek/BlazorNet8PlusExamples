using System.Runtime.InteropServices;

using BlazorNet8RenderModes.Common;

namespace BlazorNet8RenderModes.Services;

public class RenderModeServiceServer : IRenderModeService
{
    private enum RenderModeType
    {
        Static,

        InteractiveServer,

        WebAssembly,

        Prerendering,

        Transitioning,

        OutsideHttpContext,

        Unknown
    }

    private readonly IServiceProvider _serviceProvider;

    public RenderModeServiceServer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public string GetRenderMode()
    {
        var renderModeType = GetRenderModeType();
        return renderModeType switch
            {
                RenderModeType.Static => "Static SR",
                RenderModeType.InteractiveServer => "Interactive Server",
                RenderModeType.WebAssembly => "WebAssembly",
                RenderModeType.Prerendering => "Interactive Server (Prerendering)",
                RenderModeType.Transitioning => "Transitioning",
                RenderModeType.OutsideHttpContext => "Server (Outside HTTP context)",
                _ => "Unknown"
            };
    }

    private RenderModeType GetRenderModeType()
    {
        try
        {
            if (OperatingSystem.IsBrowser())
            {
                return RuntimeInformation.ProcessArchitecture == Architecture.Wasm
                           ? RenderModeType.WebAssembly
                           : RenderModeType.InteractiveServer;
            }

            var httpContextAccessor = _serviceProvider.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;
            if (httpContextAccessor?.HttpContext != null)
            {
                return httpContextAccessor.HttpContext.Response.HasStarted
                           ? RenderModeType.Prerendering
                           : RenderModeType.Static;
            }

            return RenderModeType.OutsideHttpContext;
        }
        catch (InvalidOperationException)
        {
            return RenderModeType.Transitioning;
        }
        catch (Exception)
        {
            return RenderModeType.Unknown;
        }
    }
}
