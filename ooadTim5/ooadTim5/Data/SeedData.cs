using Microsoft.AspNetCore.Identity;

namespace ooadTim5.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // ROLE
            string[] roles = { "administrator", "bibliotekar", "clan" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // ADMIN
            var admin = await userManager.FindByEmailAsync("admin@library.com");

            if (admin == null)
            {
                admin = new IdentityUser
                {
                    UserName = "admin@library.com",
                    Email = "admin@library.com",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(admin, "Admin123!");
                await userManager.AddToRoleAsync(admin, "administrator");
            }

            // BIBLIOTEKAR
            var bib = await userManager.FindByEmailAsync("bib@library.com");

            if (bib == null)
            {
                bib = new IdentityUser
                {
                    UserName = "bib@library.com",
                    Email = "bib@library.com",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(bib, "Bib123!");
                await userManager.AddToRoleAsync(bib, "bibliotekar");
            }
        }
    }
}