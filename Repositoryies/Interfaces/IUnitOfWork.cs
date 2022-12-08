namespace MyCarearApi.Repositories;

public interface IUnitOfWork : IDisposable
{
    IFreelancerHobbyRepository FreelancerHobbies { get; set; }
    ICompanyContactRepository CompanyContacts { get; set; }
    IFreelancerInformationRepository FreelancerInformations { get; set; }
    IFreelancerSkillRepository FreelancerSkills { get; set; }
    IPositionRepository Positions { get; set; }
    IHobbyRepository Hobbies { get; set; }
    ISkillRepository Skills { get; set; }
    ICompanyRepository Companies {get;}
    ICompanyLocationRepository CompanyLocations {get;}
    IExperienceRepository Experiences {get;}
    IResumeRepository Resumes {get;}
    IUserLanguageRepository UserLanguages {get;}
    int Save();
}