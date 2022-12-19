using Microsoft.EntityFrameworkCore;
using MyCarearApi.Entities;
using MyCarearApi.Entities.Enums;
using MyCarearApi.Repositories;
using MyCarearApi.Repositories.Interfaces;
using MyCarearApi.Services.JobServices.Interfaces;

namespace MyCarearApi.Services.JobServices;

public class OfferService: IOfferService
{
    private readonly IOfferRepository _offerRepository;
    private readonly IJobRepository _jobRepository;

    public OfferService(IUnitOfWork unitOfWork)
    {
        _offerRepository = unitOfWork.Offers;
        _jobRepository = unitOfWork.Jobs;
    }

    public Offer AddOffer(int jobId, int downpayment, int deadline, DeadlineRate deadlineRate, string freelancerId)
    {
        var offer = new Offer
        {
            JobId = jobId,
            Downpayment = downpayment,
            Deadline = deadline,
            DeadlineRate = deadlineRate,
            AppUserId = freelancerId,
            State = OfferState.Review
        };
        return _offerRepository.Add(offer);
    }

    public Offer GetOffer(int id) => _offerRepository.GetAll()
        .Include(x => x.Job).ThenInclude(j => j.Position)
        .Include(x => x.Job).ThenInclude(j => j.Currency)
        .Include(x => x.Job).ThenInclude(j => j.Company).FirstOrDefault();

}
