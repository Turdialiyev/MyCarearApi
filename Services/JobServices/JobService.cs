# pragma warning disable
using MyCarearApi.Entities.Enums;
using MyCarearApi.Entities;
using MyCarearApi.Repositories;
using MyCarearApi.Services.JobServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using MyCarearApi.Dtos;

namespace MyCarearApi.Services.JobServices
{
    public class JobService: IJobService
    {
        public readonly IJobRepository _jobRepository;
        public readonly IJobSkillsRepository _jobSkillsRepository;
        private readonly IJobSkillsService _jobSkillsService;
        private readonly IJobLanguagesService _jobLanguagesService;
        private readonly IPositionRepository _positionRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IOfferService _offerService;

        public JobService(IUnitOfWork unitOfWork, IJobSkillsService jobSkillsService, 
            IJobLanguagesService jobLanguagesService, IOfferService offerService)
        {
            _jobRepository = unitOfWork.Jobs;
            _jobSkillsRepository = unitOfWork.JobSkills;
            _jobSkillsService = jobSkillsService;
            _jobLanguagesService = jobLanguagesService;
            _positionRepository = unitOfWork.Positions;
            _companyRepository = unitOfWork.Companies;
            _currencyRepository = unitOfWork.Currencies;
            _offerService = offerService;
        }


        public IEnumerable<Job> Jobs => _jobRepository.GetAll()
            .Include(j => j.Currency)
            .Include(j => j.JobLanguages).ThenInclude(x => x.Language)
            .Include(j => j.JobSkills).ThenInclude(x => x.Skill)
            .Include(j => j.Position)
            .Include(j => j.Company).ThenInclude(c => c.CompanyLocations)
            .Include(j => j.Company).ThenInclude(c => c.AppUser)
            .ToList();

        public IEnumerable<Job> GetJobsOfComapany(int companyId) => _jobRepository.GetAll()
            .Where(x => x.CompanyId == companyId)
            .Include(x => x.Company).ThenInclude(x => x.AppUser)
            .Include(x => x.JobLanguages).ThenInclude(y => y.Language)
            .Include(x => x.JobSkills).ThenInclude(y => y.Skill).ToList();

        public IEnumerable<Job> GetByPage(int page, int size) => _jobRepository.GetAll()
            .Skip((page - 1) * size).Take(size)
            .Include(j => j.Currency)
            .Include(j => j.JobLanguages).ThenInclude(x => x.Language)
            .Include(j => j.JobSkills).ThenInclude(x => x.Skill)
            .Include(j => j.Position)
            .Include(j => j.Company).ThenInclude(c => c.CompanyLocations)
            .Include(j => j.Company).ThenInclude(c => c.AppUser)
            .ToList();

        public Job? GetJob(int id) => _jobRepository.GetAll().Where(x => x.Id == id)
            .Include(j => j.Currency)
            .Include(j => j.JobLanguages).ThenInclude(x => x.Language)
            .Include(j => j.JobSkills).ThenInclude(x => x.Skill)
            .Include(j => j.Position)
            .Include(j => j.Company).ThenInclude(c => c.CompanyLocations )
            .Include(j => j.Company).ThenInclude(c => c.AppUser).FirstOrDefault();


        public int AddJob(Job job) => _jobRepository.Add(job).Id;

        public int AddJob(int jobId, string name, int PositionId, int companyId)
        {
            var job = _jobRepository.GetById(jobId);
            if (job is null)
            {
                job = new Job { Id = 0, Name = name, PositionId = PositionId, IsSaved = false, CompanyId = companyId };
                return _jobRepository.Add(job).Id;
            }
            else
            {
                job.Name = name;
                job.PositionId = PositionId;
                job.CompanyId = companyId;
                return _jobRepository.Update(job).Result.Id;
            }
        }

        public async Task<int> UpdateTitle(int id,string name, int PositionId)
        {
            var job = _jobRepository.GetById(id);
            job.Name = name;
            job.PositionId = PositionId;
            var updatedJob = await _jobRepository.Update(job);
            return updatedJob.Id;
        }

        public async Task<int> AddDescriptionToJob(int id,string description, IFormFile file)
        {
            MyCarearApi.Models.Job g;
            string fileName="";
            if (file is not null)
            {
                fileName = Path.Combine("JobFiles", Guid.NewGuid().ToString() + file.FileName);
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",  fileName);
                file.CopyTo(File.Create(filePath));
            }

            var job = _jobRepository.GetById(id);
            job.Description = description;
            job.FilePath = fileName;
            var updatedJob = await _jobRepository.Update(job);
            return updatedJob.Id;
        }

        public async Task<int> SetTalantRequirements(int id, Level reuiredCandidateLevel, IEnumerable<int> requiredSkillIds,
            IEnumerable<int> requiredLanguageIds)
        {
            var job = GetJob(id);

            var requiredSkillIdsToAdd = requiredSkillIds.Where(id => !job.JobSkills.Select(x =>x.Id).Contains(id)).ToList();
            var requiredSkillIdsToDelete = job.JobSkills.Where(j => !requiredSkillIds.Contains(j.Id)).Select(j=>j.Id).ToList();

            requiredSkillIdsToAdd.ForEach(x => _jobSkillsService.Add(id, x));
            requiredSkillIdsToDelete.ForEach(x => _jobSkillsService.Delete(id, x).Wait());

            var languageIdsToAdd = requiredLanguageIds.Where(x => !job.JobLanguages.Select(y => y.Id).Contains(x)).ToList();
            var languageIdsToDelete = job.JobLanguages.Where(x => !requiredLanguageIds.Contains(x.Id)).Select(x => x.Id).ToList();

            languageIdsToAdd.ForEach(langId => _jobLanguagesService.Add(id, langId));
            languageIdsToDelete.ForEach(langId => _jobLanguagesService.Delete(id, langId).Wait());

            job.RequiredLevel= reuiredCandidateLevel;


            return (await _jobRepository.Update(job)).Id;
        }

        public async Task<int> SetTalantRequirements(int id,Level reuiredCandidateLevel, IEnumerable<Skill> requiredSkills, 
            IEnumerable<Language> requiredLanguages)
        {
            return await SetTalantRequirements(id, reuiredCandidateLevel, requiredSkills.Select(x => x.Id), requiredLanguages.Select(x => x.Id));
        }

        public async Task<int> SetContractRequirements(int id,decimal price, int currencyId, PriceRate priceRate, int deadline,
            DeadlineRate deadlineRate)
        {
            var job = GetJob(id);

            job.Price = price;
            job.PriceRate = priceRate;

            job.CurrencyId = currencyId;

            job.DeadLine = deadline;
            job.DeadlineRate = deadlineRate;

            return (await _jobRepository.Update(job)).Id;
        }

        public async Task<int> SaveJob(int id)
        {
            var job = _jobRepository.GetById(id);
            job.IsSaved = true;
            return (await _jobRepository.Update(job)).Id;
        }

        public bool IsPositionExist(int positionId) => _positionRepository.GetById(positionId) is not null;

        public Company? GetCompany(string userId) => _companyRepository.Find(x => x.AppUserId == userId).FirstOrDefault();

        public bool IsCurrencyExist(int currencyId) => _currencyRepository.GetById(currencyId) is not null;

        public async Task<int> UpdateJob(Job job)
        {
            return (await _jobRepository.Update(job)).Id;
        }

        public int GetCount() => _jobRepository.GetAll().Count();

        public bool DeleteJob(int id)
        {
            _offerService.DeleteByJob(id);
            _jobRepository.Remove(new Job { Id = id });
            return true;
        }
    }
}
