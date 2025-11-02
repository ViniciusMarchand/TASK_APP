using api.Models;
using Microsoft.AspNetCore.Identity;

namespace api.Extensions;

public static class RoleSeederExtension
{
    public static async Task SeedRolesAsync(this IServiceProvider serviceProvider)
    {
        RoleManager<IdentityRole<Guid>>? roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        UserManager<User>? userManager = serviceProvider.GetRequiredService<UserManager<User>>();

        string[] roleNames = ["admin", "manager", "member"];

        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
                await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
        }

        string adminEmail = "admin@local";
        User? adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            User user = new()
            {
                UserName = "admin",
                Email = adminEmail,
                EmailConfirmed = true,
                FirstName = "Admin",
                LastName = "User",
                Role = "admin"
            };

            IdentityResult? result = await userManager.CreateAsync(user, "Admin@123");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "admin");
            }
        }
    }
}
