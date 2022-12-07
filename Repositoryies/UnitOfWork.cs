using MyCarearApi.Data;

namespace MyCarearApi.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Companies = new CompanyRepository(context);
        CompanyLocations = new CompanyLocationRepository(context);
        Experiences = new ExperienceRepository(context);
        Resumes = new ResumeRepository(context);
        UserLanguages = new UserLanguageRepository(context);
    }

    public ICompanyRepository Companies {get;}

    public ICompanyLocationRepository CompanyLocations {get;}

    public IExperienceRepository Experiences {get;}

    public IResumeRepository Resumes {get;}

    public IUserLanguageRepository UserLanguages {get;}

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    public int Save() => _context.SaveChanges();
}