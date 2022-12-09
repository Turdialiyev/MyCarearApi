using MyCarearApi.Entities;
using MyCarearApi.Repositories;
using MyCarearApi.Repositoryies.Interfaces;
using MyCarearApi.Services.JobServices.Interfaces;

namespace MyCarearApi.Services.JobServices;

public class JobSkillsService: IJobSkillsService
{
    private readonly IJobSkillsRepository _jobSkillsRepository;

    public JobSkillsService(IUnitOfWork unitOfWork)
    {
        _jobSkillsRepository= unitOfWork.JobSkills;
    }

    public int Add(int jobId, int skillId)
    {
        return _jobSkillsRepository.Add(new JobSkill { JobId = jobId, SkillId = skillId }).Id;
    }

    public async Task Delete(int jobId, int skillId)
    {
        var jobs = _jobSkillsRepository.Find(x => x.JobId == jobId && x.SkillId == skillId);
        if (jobs is not null && jobs.Count() > 0) jobs.ToList().ForEach(async x => await _jobSkillsRepository.Remove(x));
    }
}
