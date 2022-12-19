using MyCarearApi.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace MyCarearApi.Models.JobModels
{
    public class OfferCreateModel
    {
        [Required]
        public int JobId { get; set; }
        [Required]
        public int Downpayment { get; set; }
        [Required]
        public int Deadline { get; set; }
        [Required]
        public DeadlineRate DeadlineRate { get; set; }
        [Required]
        public string FreelancerId { get; set; }
    }
}
