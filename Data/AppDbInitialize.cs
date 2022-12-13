using Microsoft.AspNetCore.Identity;
using MyCarearApi.Entities;

namespace MyCarearApi.Data;

public class AppDbInitialize
{
    public static async Task Seed(IApplicationBuilder applicationBuilder)
    {
        using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();
        var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

        using var scope = applicationBuilder.ApplicationServices
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope();

        context.Database.EnsureCreated();


        // If Countries table is not added this adds it 
        if (!context.Countries.Any())
        {
            context.Countries.AddRange(
                new List<Country>()
                {
                    new Country
                    {
                        Name = "Uzbekiston"
                    },
                    new Country
                    {
                        Name = "Russia"
                    }
                });
            context.SaveChanges();
        }

        // If Regions table is not added this adds it 
        if (!context.Regions.Any())
        {
            context.Regions.AddRange(
                new List<Region>()
                {
                    new Region
                    {
                        Name = "Amur",
                        CountryId = 2
                    },
                     new Region
                    {
                        Name = "Arkhangelsk",
                        CountryId = 2
                    },
                    new Region
                    {
                        Name = "Astrakhan",
                        CountryId = 2
                    },
                     new Region
                    {
                        Name = "Belgorod",
                        CountryId = 2
                    },
                    new Region
                    {
                        Name = "Bryansk",
                        CountryId = 2
                    },
                     new Region
                    {
                        Name = "Chelyabinsk",
                        CountryId = 2
                    },
                    new Region
                    {
                        Name = "Chuvash",
                        CountryId = 2
                    },
                     new Region
                    {
                        Name = "Irkutsk",
                        CountryId = 2
                    },
                //   List Region Of Uzbekistion
                    new Region
                    {
                      Name = "Andijon",
                      CountryId = 1
                    },
                     new Region
                    {
                      Name = "Bukhara",
                      CountryId = 1
                    },
                    new Region
                    {
                      Name = "Djizzak",
                      CountryId = 1
                    },
                     new Region
                    {
                      Name = "Fergana",
                      CountryId = 1
                    },
                    new Region
                    {
                      Name = "Kashkadarya",
                      CountryId = 1
                    },
                     new Region
                    {
                      Name = "Syrdarya",
                      CountryId = 1
                    },
                    new Region
                    {
                      Name = "Khorezm",
                      CountryId = 1
                    },
                     new Region
                    {
                      Name = "Namangan",
                      CountryId = 1
                    },
                     new Region
                    {
                      Name = "Navoi",
                      CountryId = 1
                    },
                     new Region
                    {
                      Name = "Tashkent",
                      CountryId = 1
                    },
                    new Region
                    {
                      Name = "Samarkand",
                      CountryId = 1
                    },
                     new Region
                    {
                      Name = "Surkhandarya",
                      CountryId = 1
                    },
                });
            context.SaveChanges();
        }

        // If Positions table is not added this adds it
        if (!context.Positions.Any())
        {
            context.Positions.AddRange(
                new List<Position>()
                {
                    new Position
                    {
                        Name = "Web Designer"
                    },
                    new Position
                    {
                        Name = "Backend Programmer "
                    },
                });
            context.SaveChanges();
        }

        // If Skills table is not added this adds it
        if (!context.Skills.Any())
        {
            context.Skills.AddRange(
                new List<Skill>()
                {
                    // Web Dsigner
                    new Skill
                    {
                        Name = "HTML",
                        PositionId = 1
                    },
                    new Skill
                    {
                        Name = "Figma",
                        PositionId = 1
                    },
                    new Skill
                    {
                        Name = "Prototyping",
                        PositionId = 1
                    },
                    new Skill
                    {
                        Name = "Adobe Photoshop",
                        PositionId = 1
                    },
                    // Backend programmer
                    new Skill
                    {
                        Name = "Sql",
                        PositionId = 2
                    },
                    new Skill
                    {
                        Name = "Mysqsl",
                        PositionId = 2
                    },
                    new Skill
                    {
                        Name = "LINQ",
                        PositionId = 2
                    },
                    new Skill
                    {
                        Name = "c#",
                        PositionId = 2
                    },
                });
            context.SaveChanges();
        }

        // If Languages table is not added this adds it
        if (!context.Languages.Any())
        {
            context.Languages.AddRange(
                new List<Language>()
                {
                    new Language
                    {
                        Name = "Uzb",
                    },
                    new Language
                    {
                        Name = "English"
                    },
                    new Language
                    {
                        Name = "Rus"
                    }
                });
            context.SaveChanges();
        }
    }
}