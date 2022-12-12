using Microsoft.AspNetCore.Identity;
using MyCarearApi.Entities;
using System.Text.Json;

namespace MyCarearApi
{
    public static class SeedData
    {
        public static async Task SeedUserData(IServiceProvider provider)
        {
            var userManager = provider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();

            await roleManager.CreateAsync(new IdentityRole(StaticRoles.Admin));
            await roleManager.CreateAsync(new IdentityRole(StaticRoles.Company));
            await roleManager.CreateAsync(new IdentityRole(StaticRoles.Freelancer));

            var admin = new AppUser { UserName = "Admin", Email = "admin@gmail.com" };
            var adminPassword = "lkjhDSAQre123$";
            var result = await userManager.CreateAsync(admin, adminPassword);
            Console.WriteLine(JsonSerializer.Serialize(result));
        }

        public static List<Position> DefaultPositions { get; set; }

        public static List<Language> DefaultLanguages { get; set; }

        public static List<Hobby> DefaultHobbies { get; set; }

        public static List<Currency> DefaultCurrencies { get; set; }
    }
}
