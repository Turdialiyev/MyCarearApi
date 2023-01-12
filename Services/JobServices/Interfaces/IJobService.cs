using MyCarearApi.Entities;
using MyCarearApi.Entities.Enums;

namespace MyCarearApi.Services.JobServices.Interfaces;

public interface IJobService
{
    IEnumerable<Job> Jobs { get; }

    IEnumerable<Job> GetJobsOfComapany(int companyId);

    IEnumerable<Job> GetByPage(int page, int size);

    Job GetJob(int id);

    int AddJob(Job job);

    int AddJob(int jobId, string name, int PositionId, int companyId);

    int GetCount();

    Task<int> UpdateTitle(int id, string name, int PositionId);

    Task<int> AddDescriptionToJob(int id, string Description, IFormFile file);

    Task<int> SetTalantRequirements(int id, Level reuiredCandidateLevel, IEnumerable<int> requiredSkillIds, IEnumerable<int> requiredLanguageIds);

    Task<int> SetTalantRequirements(int id, Level reuiredCandidateLevel, IEnumerable<Skill> requiredSkills, IEnumerable<Language> requiredLanguages);

    Task<int> SetContractRequirements(int id, decimal price, int currencyId, PriceRate priceRate, int deadline, DeadlineRate deadlineRate);
    
    Task<int> SaveJob(int id);

    bool IsPositionExist(int positionId);

    public Company GetCompany(string userId);

    bool IsCurrencyExist(int currencyId);

    Task<int> UpdateJob(Job job);

    bool DeleteJob(int id);
}
