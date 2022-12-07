namespace MyCarearApi.Repositories;

public interface IUnitOfWork : IDisposable
{
    ICompanyRepository Companies {get;}
    ICompanyLocationRepository CompanyLocations {get;}
    IExperienceRepository Experiences {get;}
    IResumeRepository Resumes {get;}
    IUserLanguageRepository UserLanguages {get;}
    int Save();
}