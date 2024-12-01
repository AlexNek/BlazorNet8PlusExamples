using System.Security.Claims;

using BlazorAuthentication.Client;
using BlazorAuthentication.Components.Account.Pages;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAuthentication.Controllers
{
    /// <summary>
    /// Don't used now!
    /// Class AccountController Example.
    /// Implements the <see cref="Controller" />
    /// Just another example for authentication way
    /// </summary>
    /// <seealso cref="Controller" />
    [Route("api/[controller]/[action]")]
    public class AccountControllerExample : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] string email, [FromForm] string password)
        {
            if (email == LoginPage.FixedEmail && password == LoginPage.FixedPassword)
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, email) };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));
                var cookieOptions = new CookieOptions { Expires = DateTimeOffset.UtcNow.AddDays(1), Secure = true };
                Response.Cookies.Append("APITest", "Test value", cookieOptions);

                //server-side redirect (Redirect method) inherently causes a full page load.
                return Redirect("/");
            }

            return Unauthorized();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }

        [HttpGet]
        public IActionResult GetUser()
        {
            var user = new User
                           {
                               Email = User.Identity?.Name,
                               IsAuthenticated = User.Identity?.IsAuthenticated ?? false,
                               Roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value)
                           };

            return Ok(user);
        }
    }
}