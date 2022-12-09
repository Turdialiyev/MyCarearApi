﻿using MyCarearApi.Entities.Enums;
using MyCarearApi.Entities;
using MyCarearApi.Repositories;
using MyCarearApi.Repositoryies.Interfaces;
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

        public JobService(IUnitOfWork unitOfWork, IJobSkillsService jobSkillsService, IJobLanguagesService jobLanguagesService)
        {
            _jobRepository = unitOfWork.Jobs;
            _jobSkillsRepository = unitOfWork.JobSkills;
            _jobSkillsService = jobSkillsService;
            _jobLanguagesService = jobLanguagesService;
        }

        IEnumerable<Job>? Jobs => _jobRepository.GetAll()
            .Include(j => j.Currency)
            .Include(j => j.JobLanguages)
            .Include(j => j.JobSkills)
            .Include(j => j.Position)
            .Include(j => j.Company).ThenInclude(c => c.CompanyLocations)
            .Include(j => j.Company).ThenInclude(c => c.AppUser)
            .ToList();

        Job? GetJob(int id) => _jobRepository.GetAll()
            .Include(j => j.Currency)
            .Include(j => j.JobLanguages)
            .Include(j => j.JobSkills)
            .Include(j => j.Position)
            .Include(j => j.Company).ThenInclude(c => c.CompanyLocations )
            .Include(j => j.Company).ThenInclude(c => c.AppUser).FirstOrDefault();


        int AddJob(Job job) => _jobRepository.Add(job).Id;

        int AddJob(string name, int PositionId)
        {
            var job = new Job {Id = 0, Name = name, PositionsId = PositionId };
            return _jobRepository.Add(job).Id;
        }

        async Task<int> UpdateTitle(int id,string name, int PositionId)
        {
            var job = _jobRepository.GetById(id);
            job.Name = name;
            job.PositionsId = PositionId;
            var updatedJob = await _jobRepository.Update(job);
            return updatedJob.Id;
        }

        public async Task<int> AddDescriptionToJob(int id,string description, IFormFile file)
        {
            string fileName="";
            if (file is not null)
            {
                fileName = Guid.NewGuid().ToString() + file.FileName;
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "JobFiles", fileName);
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
            requiredSkillIdsToDelete.ForEach(async x => await _jobSkillsService.Delete(id, x));

            var languageIdsToAdd = requiredLanguageIds.Where(x => !job.JobLanguages.Select(y => y.Id).Contains(x)).ToList();
            var languageIdsToDelete = job.JobLanguages.Where(x => !requiredLanguageIds.Contains(x.Id)).Select(x => x.Id).ToList();

            languageIdsToAdd.ForEach(langId => _jobLanguagesService.Add(id, langId));
            languageIdsToDelete.ForEach(async langId => await _jobLanguagesService.Delete(id, langId));

            job.RequiredLevel= reuiredCandidateLevel;

            return (await _jobRepository.Update(job)).Id;
        }

        public int SetTalantRequirements(int id,Level reuiredCandidateLevel, IEnumerable<Skill> requiredSkills, 
            IEnumerable<Language> requiredLanguages)
        {

        }

        int SetContractRequirements(int id,decimal price, Currency currency, PriceRate priceRate, int deadline, DeadlineRate deadlineRate);

        void UpdateJob(Job job);

    }
}