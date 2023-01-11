using MyCarearApi.Entities.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyCarearApi.Models.JobModels
{
    [Description]
    public class ContractRequirementsModel
    {
        [Required]
        public int JobId { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int CurrencyId { get; set; }
        [Required]
        public PriceRate PriceRate { get; set; }
        [Required]
        public int Deadline { get; set; }
        [Required]
        public DeadlineRate DeadlineRate { get; set; }
    }
}
