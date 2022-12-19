using MyCarearApi.Entities;
using MyCarearApi.Entities.Enums;

namespace MyCarearApi.Services.JobServices.Interfaces;

public interface IOfferService
{
    Offer AddOffer(int jobId, int downpayment, int deadline, DeadlineRate deadlineRate, string freelancerId);
    Offer GetOffer(int id);
}
