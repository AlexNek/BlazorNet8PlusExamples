using Microsoft.AspNetCore.Identity;

namespace BlazorAuthenticationV2.Data;

public static class DbInitializer
{
    public static async Task SeedUsersAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (!context.Users.Any())
        {
            var users = new List<ApplicationUser>
                            {
                                new ApplicationUser { Id = "6a2e2186-df70-43a4-a76e-9dd081fc81c9", UserName = "user1", Email = "user1@example.com", EmailConfirmed = true },
                                new ApplicationUser { Id = "9C40BD82-DA25-405E-8C4D-FECAD97345B8", UserName = "user2", Email = "user2@example.com", EmailConfirmed = true }
                            };

            foreach (var user in users)
            {
                await userManager.CreateAsync(user, "Password123!");
            }

            await context.SaveChangesAsync();
        }
    }
}