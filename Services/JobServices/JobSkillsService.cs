# pragma warning disable
using MyCarearApi.Entities;
using MyCarearApi.Repositories;
using MyCarearApi.Services.JobServices.Interfaces;

namespace MyCarearApi.Services.JobServices;

public class JobSkillsService: IJobSkillsService
{
    private readonly IJobSkillsRepository _jobSkillsRepository;
    private ISkillRepository _skillRepository;

    public JobSkillsService(IUnitOfWork unitOfWork)
    {
        _jobSkillsRepository= unitOfWork.JobSkills;
        _skillRepository = unitOfWork.Skills;
    }

    public int Add(int jobId, int skillId)
    {
        return _jobSkillsRepository.Add(new JobSkill { JobId = jobId, SkillId = skillId }).Id;
    }


    public IEnumerable<int> CheckSkillIds(IEnumerable<int> ids)
    {
        var existIds = _skillRepository.GetAll().Select(x => x.Id).ToList();
        return ids.Where(id => !existIds.Contains(id));
    }

    public async Task Delete(int jobId, int skillId)
    {
        var jobs = _jobSkillsRepository.Find(x => x.JobId == jobId && x.SkillId == skillId);
        if (jobs is not null && jobs.Count() > 0) jobs.ToList().ForEach(async x => await _jobSkillsRepository.Remove(x));
    }
}
