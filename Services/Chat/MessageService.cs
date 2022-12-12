using Microsoft.AspNetCore.Identity;
using MyCarearApi.Entities;
using MyCarearApi.Repositories;
using System.Collections;

namespace MyCarearApi.Services.Chat
{
    public class MessageService: IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IFreelancerInformationRepository _freelancerInformationRepository;
        private readonly UserManager<AppUser> _userManager;

        public MessageService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _messageRepository = unitOfWork.Messages;
            _userManager = userManager;
            _companyRepository = unitOfWork.Companies;
            _freelancerInformationRepository = unitOfWork.FreelancerInformations;
        }

        public Message AddMessage(Message message)
        {
            return _messageRepository.Add(message);
        }

        public void RemoveMessage(int Id)
        {
            _messageRepository.Remove(new Message { Id = Id });
        }

        public async Task<Message> UpdateMessage(Message message) => await _messageRepository.Update(message);

        public IList GetChats(string userId)
        {
            var userIds = _messageRepository.Find(x => x.FromId == userId).Select(x => x.ToId).Distinct().ToList();
            return _userManager.Users.Where(u => userIds.Contains(u.Id)).Select(x => new
            {
                x.Id,
                x.UserName,
                x.FirstName,
                x.LastName,
                x.Email,
                Img = PhotoUrl(userId)
            }).ToList();
        }

        private async Task<string> PhotoUrl(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var photoPath = "";
            if(await _userManager.IsInRoleAsync(user, StaticRoles.Company))
            {
                var company = _companyRepository.GetAll().FirstOrDefault(x => x.AppUserId == userId);
                if (company is not null) photoPath = company.Photo;
                if (!File.Exists(photoPath))
                {
                    photoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "DefaultUser.png");
                }
            }
            if(await _userManager.IsInRoleAsync(user, StaticRoles.Freelancer))
            {
                var freelancerInfo = _freelancerInformationRepository.GetAll().FirstOrDefault(x => x.AppUserId == userId);
                if (freelancerInfo is not null) photoPath = freelancerInfo.FreelancerImage;
                if (!File.Exists(photoPath))
                {
                    photoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "DefaultCompany.png");
                }
            }
            return photoPath;
        }
    }
}
