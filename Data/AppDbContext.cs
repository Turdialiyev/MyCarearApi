using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyCarearApi.Entities;
using MyCareerApi.Entities;

namespace MyCarearApi.Data;


public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Position>().HasData(
            new Position
            {
                Id = 1,
                Name = "Web Desigin",
            },
            new Position
            {
                Id = 2,
                Name = "Web Backend",
            });

        modelBuilder.Entity<Skill>().HasData(
            new Skill
            {
                Id = 1,
                Name = "Html",
                PositionId = 1,

            },
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
                Name = "Figma",
                PositionId = 1,
            });
    }
}



