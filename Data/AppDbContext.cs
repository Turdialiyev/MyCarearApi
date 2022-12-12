using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyCarearApi.Entities;
using MyCareerApi.Entities;

namespace MyCarearApi.Data;


public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<FreelancerHobby> FreelancerHobbies { get; set; }
    public DbSet<Region> Regions { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Address> Addresses { get; set; }

    public DbSet<Company> Companys { get; set; }

    public DbSet<CompanyLocation> CompanyLocations { get; set; }

    public DbSet<Contact> Contacts { get; set; }

    public DbSet<Contract> Contracts { get; set; }

    public DbSet<Education> Educations { get; set; }

    public DbSet<Experience> Experiences { get; set; }

    public DbSet<FreelancerInformation> FreelancerInformations { get; set; }

    public DbSet<FreelancerSkill> FreelancerSkills { get; set; }

    public DbSet<Hobby> Hobbies { get; set; }

    public DbSet<Job> Jobs { get; set; }

    public DbSet<JobSkill> JobsSkill { get; set; }

    public DbSet<Position> Positions { get; set; }

    public DbSet<Resume> Resumes { get; set; }

    public DbSet<Skill> Skills { get; set; }

    public DbSet<UserLanguage> UserLanguages { get; set; }

    public DbSet<Language> Languages { get; set; }

    public DbSet<JobLanguage> JobLanguages { get; set; }

    public DbSet<Currency> Currencys { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Language>().HasData(
            new Language
            {
                Id = 1,
                Name = "English",
                LanguageCode = "bilmadim"
            },
            new Language
            {
                Id = 2,
                Name = "Russion",
                LanguageCode = "bilmadim"
            },
            new Language
            {
                Id = 3,
                Name = "Uzbek",
                LanguageCode = "bilmadim"
            });

        builder.Entity<Hobby>().HasData(
            new Hobby
            {
                Id = 1,
                Name = "Football"
            },
            new Hobby
            {
                Id = 2,
                Name = "Play tenis"
            },
            new Hobby
            {
                Id = 3,
                Name = "Reading"
            },
            new Hobby
            {
                Id = 4,
                Name = "listening"
            }

            );

        builder.Entity<Position>().HasData(
            new Position
            {
                Id = 1,
                Name = "Web Designer"
            });

        builder.Entity<Skill>().HasData(
            new Skill
            {
                Id = 1,
                Name = "Design",
                PositionId = 1,
            });
        builder.Entity<Skill>().HasData(
            new Skill
            {
                Id = 2,
                Name = "Figma",
                PositionId = 1,
            },
            new Skill
            {
                Id = 3,
                Name = "Html",
                PositionId = 1,
            },
            new Skill
            {
                Id = 4,
                Name = "Adobe",
                PositionId = 1,
            }
            );

        builder.Entity<Country>().HasData(
            new Country
            {
                Id = 1,
                Name = "Uzbekistion"
            });
        builder.Entity<Region>().HasData(
            new Region
            {
                Id = 1,
                Name = "Andijon",
                CountryId = 1
            });
        builder.Entity<Region>().HasData(
            new Region
            {
                Id = 2,
                Name = "Farg'ona",
                CountryId = 1
            });
    }
}



