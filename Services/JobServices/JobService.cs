using MyCarearApi.Repositories;
using MyCarearApi.Repositoryies.Interfaces;

namespace MyCarearApi.Services.JobServices
{
    public class JobService
    {
        public readonly IJobRepository _jobRepository;
        public readonly IJobSkillsRepository _jobSkillsRepository;

        public JobService(IUnitOfWork unitOfWork)
        {
            
        }
    }
}
