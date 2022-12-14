using MyCarearApi.Data;
using MyCarearApi.Repositories.Interfaces;

namespace MyCarearApi.Repositories;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        ProjectImages = new ProjectImageRepository(context);
        FreelancerPortfolios = new FreelancerPortfolioRepository(context);
        FreelancerProjects = new FreelancerProjectRepository(context);
        Countries = new CountryRepository(context);
        FreelancerContacts = new FreelancerContactRepository(context);
        Educations = new EducationRepository(context);
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
        Addresses = new AddressRepository(context);
        Jobs = new JobRepository(context);
        JobSkills = new JobSkillsRepository(context);
        Contracts = new ContractRepository(context);
        JobLanguages = new JobLanguageRepository(context);
        Languages= new LanguageRepository(context);
        Currencies = new CurrencyRepository(context);
        Messages = new MessageRepository(context);
        Chats = new ChatRepository(context);
        ChatFiles = new ChatFileRepository(context);
        Offers = new OfferRepository(context);
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
    public IEducationRepository Educations { get; set; }
    public IFreelancerContactRepository FreelancerContacts { get; set; }

    public IJobRepository Jobs { get; }

    public IJobSkillsRepository JobSkills { get; }


    public IContractRepository Contracts { get; }

    public IJobLanguageRepository JobLanguages { get; }
    public ILanguageRepository Languages { get; }
    public ICurrencyRepository Currencies { get; }
    public IMessageRepository Messages { get; }

    public ICountryRepository Countries { get; set; }

    public IChatRepository Chats { get; }
    public IFreelancerProjectRepository FreelancerProjects { get; set; }
    public IProjectImageRepository ProjectImages { get; set; }
    public IFreelancerPortfolioRepository FreelancerPortfolios { get; set; }

    public IChatFileRepository ChatFiles { get; }

    public IOfferRepository Offers { get; }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    public int Save() => _context.SaveChanges();
}