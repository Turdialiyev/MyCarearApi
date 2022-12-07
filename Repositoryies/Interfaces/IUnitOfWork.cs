namespace MyCarearApi.Repositories;

public interface IUnitOfWork : IDisposable
{
    ICompanyContactRepository CompanyContacts { get; set; }
    IFreelancerInformationRepository FreelancerInformations { get; set; }
    IFreelancerSkillRepository FreelancerSkills { get; set; }
    IPositionRepository Positions { get; set; }
    IHobbyRepository Hobbies { get; set; }
    IPositionSkillRepository PositionSkills { get; set; }
    ISkillRepository Skills { get; set; }
    ICompanyRepository Companies {get;}
    ICompanyLocationRepository CompanyLocations {get;}
    IExperienceRepository Experiences {get;}
    IResumeRepository Resumes {get;}
    IUserLanguageRepository UserLanguages {get;}
    int Save();
}