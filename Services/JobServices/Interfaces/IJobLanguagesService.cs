namespace MyCarearApi.Services.JobServices.Interfaces
{
    public interface IJobLanguagesService
    {
        int Add(int jobId, int languageId);

        IEnumerable<int> CheckLanguageIds(IEnumerable<int> ids);

        Task Delete(int jobId, int languageId);
    }
}
