using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;

namespace BlazorAuthentication.Components.Account.Pages;

public partial class LoginPage
{
    public const string FixedEmail = "user@example.com";
    public const string FixedPassword = "password";

    private readonly LoginModel _loginModel = new()
                                                  {
                                                      Email = FixedEmail, Password = FixedPassword
                                                  };

    private string? _errorMessage;

    [CascadingParameter]
    private HttpContext HttpContext { get; set; } = default!;

    

    private async Task HandleLogin()
    {
        if (_loginModel.Email == FixedEmail
            && _loginModel.Password == FixedPassword)
        {
            var claims = new List<Claim>
                             {
                                 new Claim(ClaimTypes.Name, "Demo"),
                                 new Claim(ClaimTypes.NameIdentifier, "123456789"),
                                 new Claim(ClaimTypes.Email, _loginModel.Email),
                             };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
                                     {
                                         IsPersistent = true,
                                         ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1)
                                     };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            Navigation.NavigateTo("/user-test-interactive", true);
        }
        else
        {
            _errorMessage = "Invalid login attempt.";
        }
    }

    private class LoginModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
