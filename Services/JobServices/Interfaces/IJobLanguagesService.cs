namespace MyCarearApi.Services.JobServices.Interfaces
{
    public interface IJobLanguagesService
    {
        int Add(int jobId, int languageId);

        Task Delete(int jobId, int languageId);
    }
}
