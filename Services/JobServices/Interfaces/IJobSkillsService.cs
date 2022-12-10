using MyCarearApi.Entities;

namespace MyCarearApi.Services.JobServices.Interfaces;

public interface IJobSkillsService
{
    int Add(int jobId, int skillId);

    IEnumerable<int> CheckSkillIds(IEnumerable<int> ids);

    Task Delete(int jobId, int skillId);
}
