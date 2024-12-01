using System.Diagnostics;
using System.Security.Claims;

using BlazorAuthentication.Client;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace BlazorAuthentication
{
    // This is a server-side AuthenticationStateProvider that revalidates the security stamp for the connected user
    // every 30 minutes an interactive circuit is connected. It also uses PersistentComponentState to flow the
    // authentication state to the client which is then fixed for the lifetime of the WebAssembly application.
    internal sealed class PersistingRevalidatingAuthenticationStateProvider : RevalidatingServerAuthenticationStateProvider
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly PersistentComponentState _state;
        private readonly IdentityOptions _options;

        private readonly PersistingComponentStateSubscription _subscription;

        private Task<AuthenticationState>? _authenticationStateTask;

        public PersistingRevalidatingAuthenticationStateProvider(
            ILoggerFactory loggerFactory,
            IServiceScopeFactory serviceScopeFactory,
            PersistentComponentState persistentComponentState,
            IOptions<IdentityOptions> optionsAccessor)
            : base(loggerFactory)
        {
            _scopeFactory = serviceScopeFactory;
            _state = persistentComponentState;
            _options = optionsAccessor.Value;

            AuthenticationStateChanged += OnAuthenticationStateChanged;
            _subscription = _state.RegisterOnPersisting(OnPersistingAsync, RenderMode.InteractiveWebAssembly);
        }

        protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(30);

        protected override async Task<bool> ValidateAuthenticationStateAsync(
            AuthenticationState authenticationState, CancellationToken cancellationToken)
        {
            // Extract the ClaimsPrincipal from the authentication state
            var user = authenticationState.User;

            // Check if the user is authenticated
            if (user.Identity is { IsAuthenticated: false })
            {
                return false; // User is not authenticated, so the state is invalid
            }

            // Optionally, validate specific claims
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return false; // No user ID claim found, invalid state
            }

            // Get the user manager from a new scope to ensure it fetches fresh data
            //await using var scope = scopeFactory.CreateAsyncScope();
            //var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            //return await ValidateSecurityStampAsync(userManager, authenticationState.User);
            // TODO check user state, possible check tokens
            return true;
        }
        
        private void OnAuthenticationStateChanged(Task<AuthenticationState> task)
        {
            _authenticationStateTask = task;
        }

        private async Task OnPersistingAsync()
        {
            if (_authenticationStateTask is null)
            {
                throw new UnreachableException($"Authentication state not set in {nameof(OnPersistingAsync)}().");
            }

            var authenticationState = await _authenticationStateTask;
            var principal = authenticationState.User;

            if (principal.Identity?.IsAuthenticated == true)
            {
                var userId = principal.FindFirst(_options.ClaimsIdentity.UserIdClaimType)?.Value;
                var email = principal.FindFirst(_options.ClaimsIdentity.EmailClaimType)?.Value;
                var name = principal.FindFirst(_options.ClaimsIdentity.UserNameClaimType)?.Value;
                IEnumerable<Claim> roles = principal.FindAll(_options.ClaimsIdentity.RoleClaimType);

                string rolesStr = string.Join(",", roles.Select(item => item.Value));
                if (userId != null && email != null)
                {
                    _state.PersistAsJson(nameof(UserInfo), new UserInfo
                    {
                        UserId = userId,
                        Email = email,
                        Name = name,
                        Roles = rolesStr
                    });
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            _subscription.Dispose();
            AuthenticationStateChanged -= OnAuthenticationStateChanged;
            base.Dispose(disposing);
        }
    }
}
