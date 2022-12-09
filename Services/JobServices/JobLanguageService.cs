using MyCarearApi.Entities;
using MyCarearApi.Repositoryies.Interfaces;
using MyCarearApi.Services.JobServices.Interfaces;

namespace MyCarearApi.Services.JobServices;

public class JobLanguageService: IJobLanguagesService
{
    public IJobLanguageRepository _jobLanguageRepository;

    public JobLanguageService(IJobLanguageRepository jobLanguageRepository)
    {
        _jobLanguageRepository = jobLanguageRepository;
    }

    public int Add(int jobId, int languageId)
    {
        var jobLanguage = new JobLanguage { JobId = jobId, LanguageId = languageId };
        return _jobLanguageRepository.Add(jobLanguage).Id;

    }

    public async Task Delete(int jobId, int languageId)
    {
        var jobLanguage = _jobLanguageRepository.Find(x => x.LanguageId == languageId && x.JobId == jobId).FirstOrDefault();
        if(jobLanguage is not null)
        {
            await _jobLanguageRepository.Remove(jobLanguage);
        }
    }
}
