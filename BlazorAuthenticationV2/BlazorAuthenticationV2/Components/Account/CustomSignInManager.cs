using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace BlazorAuthenticationV2.Components.Account;

public class CustomSignInManager<TUser> : SignInManager<TUser> where TUser : class
{
    public CustomSignInManager(UserManager<TUser> userManager,
                               IHttpContextAccessor contextAccessor,
                               IUserClaimsPrincipalFactory<TUser> claimsFactory,
                               IOptions<IdentityOptions> optionsAccessor,
                               ILogger<SignInManager<TUser>> logger,
                               IAuthenticationSchemeProvider schemes,
                               IUserConfirmation<TUser> confirmation)
        : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
    {
    }

    public override async Task<SignInResult> PasswordSignInAsync(string email, string password, bool isPersistent, bool lockoutOnFailure)
    {
        var user = await UserManager.FindByEmailAsync(email);
        if (user == null)
        {
            return SignInResult.Failed;
        }

        return await PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
    }
}