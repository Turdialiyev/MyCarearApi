using MyCarearApi.Entities.Enums;

namespace MyCarearApi.Entities;

public class Offer
{
    public int Id { get; set; }

    public int JobId { get; set; }
    public Job Job { get; set; }

    /// <summary>
    /// FreelancerId
    /// </summary>
    public string AppUserId { get; set; }
    /// <summary>
    /// Freelancer
    /// </summary>
    public AppUser AppUser { get; set; }

    public int Downpayment { get; set; } = 0;

    public int Deadline { get; set; }

    public DeadlineRate DeadlineRate { get; set; }

    public OfferState State { get; set; }

}
