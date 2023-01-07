using MyCarearApi.Entities.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyCarearApi.Models.JobModels
{
    [Description]
    public class ContractRequirementsModel
    {
        [Required]
        public int JobId;
        [Required]
        public decimal Price;
        [Required]
        public int CurrencyId;
        [Required]
        public PriceRate PriceRate;
        [Required]
        public int Deadline;
        [Required]
        public DeadlineRate DeadlineRate;
    }
}
