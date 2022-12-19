using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using MyCarearApi.Entities;
using MyCareerApi.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace MyCarearApi.Data;


public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Country> Countries { get; set; }
    public DbSet<Region> Regions { get; set; }
    public DbSet<FreelancerContact> Connections { get; set; }
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

    public DbSet<Message> Messages { get; set; }


}



