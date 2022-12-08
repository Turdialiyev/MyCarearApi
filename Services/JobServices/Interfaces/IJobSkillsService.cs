namespace MyCarearApi.Services.JobServices.Interfaces;

public interface IJobSkillsService
{
    int Add(int jobId, int skillId);

    Task Delete(int jobId, int skillId);
}
