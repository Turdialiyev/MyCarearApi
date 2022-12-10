using MyCarearApi.Entities;
using MyCarearApi.Repositories;
using MyCarearApi.Repositoryies.Interfaces;
using MyCarearApi.Services.JobServices.Interfaces;

namespace MyCarearApi.Services.JobServices;

public class JobLanguageService: IJobLanguagesService
{
    private IJobLanguageRepository _jobLanguageRepository;
    private ILanguageRepository _languageRepository;

    public JobLanguageService(IUnitOfWork unitOfWork)
    {
        _jobLanguageRepository = unitOfWork.JobLanguages;
        _languageRepository = unitOfWork.Languages;
    }

    public int Add(int jobId, int languageId)
    {
        var jobLanguage = new JobLanguage { JobId = jobId, LanguageId = languageId };
        return _jobLanguageRepository.Add(jobLanguage).Id;

    }

    public IEnumerable<int> CheckLanguageIds(IEnumerable<int> ids)
    {
        IEnumerable<int> existLanguageIds = _languageRepository.GetAll().Select(x => x.Id).ToList();
        return ids.Where(id => !existLanguageIds.Contains(id));
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
