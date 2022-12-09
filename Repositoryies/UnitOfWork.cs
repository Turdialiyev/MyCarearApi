using MyCarearApi.Data;
using MyCarearApi.Repositoryies;
using MyCarearApi.Repositoryies.Interfaces;

namespace MyCarearApi.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        FreelancerHobbies = new FreelancerHobbyRepository(context);
        Companies = new CompanyRepository(context);
        CompanyLocations = new CompanyLocationRepository(context);
        Experiences = new ExperienceRepository(context);
        Resumes = new ResumeRepository(context);
        UserLanguages = new UserLanguageRepository(context);
        FreelancerInformations = new FreelancerInformationRepository(context);
        FreelancerSkills = new FreelancerSkillRepository(context);
        Positions = new PositionRepository(context);
        Hobbies = new HobbyRepository(context);
        Skills = new SkillRepository(context);
        CompanyContacts = new CompanyContactRepository(context);
        Jobs = new JobRepository(context);
        JobSkills = new JobSkillsRepository(context);
        Addresses = new AddressRepository(context);
        Contracts = new ContractRepository(context);
    }

    public ICompanyRepository Companies { get; }

    public ICompanyLocationRepository CompanyLocations { get; }

    public IExperienceRepository Experiences { get; }

    public IResumeRepository Resumes { get; }

    public IUserLanguageRepository UserLanguages { get; }
    public IFreelancerInformationRepository FreelancerInformations { get; set; }
    public IFreelancerSkillRepository FreelancerSkills { get; set; }
    public IPositionRepository Positions { get; set; }
    public IHobbyRepository Hobbies { get; set; }
    public ISkillRepository Skills { get; set; }
    public ICompanyContactRepository CompanyContacts { get; set; }
    public IFreelancerHobbyRepository FreelancerHobbies { get; set; }
    public IAddressRepository Addresses { get; set; }

    public IJobRepository Jobs { get; }

    public IJobSkillsRepository JobSkills { get; }

    public IContractRepository Contracts { get; }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    public int Save() => _context.SaveChanges();
}