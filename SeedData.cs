# pragma warning disable
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
            await userManager.AddToRoleAsync(await userManager.FindByEmailAsync(admin.Email), StaticRoles.Admin);

            var user = new AppUser { UserName = "User", Email = "user@gmail.com" };
            var userPassword = "lkjhDSAQre123$";
            result = await userManager.CreateAsync(user, userPassword);
            await userManager.AddToRoleAsync(await userManager.FindByEmailAsync(admin.Email), StaticRoles.Freelancer);

            var test = new AppUser { UserName = "Test", Email = "test@gmail.com" };
            var testPassword = "lkjhDSAQre123$";
            result = await userManager.CreateAsync(test, testPassword);
            await userManager.AddToRoleAsync(await userManager.FindByEmailAsync(user.Email), StaticRoles.Company);


            Console.WriteLine(JsonSerializer.Serialize(result));
        }
        /*{
     "email": "string@gmail.com",
     "password": "stringS1!",
     "confirmPassword": "stringS1!"
   }*/
        /*{
      "email": "sultonxon@gmail.com",
      "password": "stringS1!",
      "confirmPassword": "stringS1!"
    }*/

        public static List<Position> DefaultPositions { get; set; }

        public static List<Language> DefaultLanguages { get; set; }

        public static List<Hobby> DefaultHobbies { get; set; }

        public static List<Currency> DefaultCurrencies { get; set; }
    }
}
