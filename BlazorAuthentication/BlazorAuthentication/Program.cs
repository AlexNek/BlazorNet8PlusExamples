using BlazorAuthentication.Client;
using BlazorAuthentication.Components;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorAuthentication
{
    public class Program
    {
        public const string? AuthCookieName = "TestAppCookieName";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var services = builder.Services;
            services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents();

            // Read base address from configuration
            var baseAddress = builder.Configuration["ApiSettings:BaseAddress"];
            services.AddHttpClient<ApiAuthenticationStateProviderExample>( client => { client.BaseAddress = new Uri(baseAddress); });

            services.AddControllers();

            services.AddCascadingAuthenticationState();
            services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

            

            services.AddAuthentication(options =>
                    {
                        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    })
                .AddCookie(options =>
                    {
                        options.Cookie.Name = AuthCookieName;
                        
                        options.SlidingExpiration = true;
                        options.LoginPath = "/login";
                        options.AccessDeniedPath = "/access-denied";

                    });

            services.AddAuthorization();
            

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(BlazorAuthentication.Client._Imports).Assembly);

            app.MapControllers();
            app.MapFallbackToFile("index.html");
            app.MapLogout(AuthCookieName);
            app.Run();
        }
    }
}
