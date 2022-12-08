using MyCarearApi.Entities;
using MyCarearApi.Entities.Enums;

namespace MyCarearApi.Services.JobServices.Interfaces;

public interface IJobService
{
    IEnumerable<Job> Jobs { get; }

    Job GetJob(int id);

    int AddJob(Job job);

    int AddJob(string name, int PositionId);

    int UpdateTitle(int id, string name, int PositionId);

    int AddDescriptionToJob(int id, string Description, IFormFile file);

    int SetTalantRequirements(int id, Level reuiredCandidateLevel, IEnumerable<int> requiredSkillIds, IEnumerable<int> requiredLanguageIds);

    int SetTalantRequirements(int id, Level reuiredCandidateLevel, IEnumerable<Skill> requiredSkills, IEnumerable<Language> requiredLanguages);

    int SetContractRequirements(int id, decimal price, Currency currency, PriceRate priceRate, int deadline, DeadlineRate deadlineRate);
    
    void UpdateJob(Job job);

}
