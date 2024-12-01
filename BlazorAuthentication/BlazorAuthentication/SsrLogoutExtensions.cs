using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace BlazorAuthentication;

public static class SsrLogoutExtensions
{
    public static void MapLogout(this WebApplication app, string cookieName)
    {
        app.MapPost(
            "/logout-endpoint",
             async (
                HttpContext httpContext,
                [FromForm] string? returnUrl) =>
                {
                    // Sign out the user
                    await httpContext.SignOutAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme);

                    // If no return URL is specified, default to the home page
                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        returnUrl = "/";
                    }

                    // Ensure the returnUrl starts with a forward slash if it's not empty
                    if (!returnUrl.StartsWith("/"))
                    {
                        returnUrl = "/" + returnUrl;
                    }

                    //return Results.LocalRedirect($"~{returnUrl}");
                    //TypedResults.LocalRedirect($"~{returnUrl}");
                    //return Results.Redirect($"{returnUrl}");
                    //return Results.Redirect("/login");

                    // Causes a full page reload.
                    // This reload will cause the Blazor app to reinitialize and fetch the updated authentication state, effectively logging out the user on the client side.
                    // Set the Location header manually
                    httpContext.Response.Headers["Location"] = $"{returnUrl}";
                    httpContext.Response.StatusCode = StatusCodes.Status302Found;

                    // Log the response status code
                    httpContext.Response.OnStarting(() =>
                        {
                            Console.WriteLine($"Response status code: {httpContext.Response.StatusCode}");
                            return Task.CompletedTask;
                        });

                    return Task.CompletedTask;
                });

        app.MapPost(
           "/logout-http",
           [IgnoreAntiforgeryToken] async (
               HttpContext httpContext) =>
           {
               // Sign out the user
               await httpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);

               // Explicitly delete the authentication cookie
               //httpContext.Request.Cookies - accesses the cookies sent by the client(browser) in the request.
               //httpContext.Response.Cookies accesses the cookies being set or deleted in the response sent back to the client.
               //httpContext.Response.Cookies.Delete(cookieName);

               //if (httpContext.Request.Cookies.ContainsKey(cookieName))
               //{
               //    httpContext.Response.Cookies.Delete(cookieName);
               //    Console.WriteLine($"{cookieName} deleted.");
               //}
               //else
               //{
               //    Console.WriteLine($"{cookieName} not found.");
               //}

               

               return Task.CompletedTask;
           }).DisableAntiforgery();
    }
}
