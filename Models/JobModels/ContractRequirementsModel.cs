using MyCarearApi.Entities.Enums;

namespace MyCarearApi.Models.JobModels
{
    public class ContractRequirementsModel
    {
        public int JobId;
        public decimal Price;
        public int CurrencyId;
        public PriceRate PriceRate;
        public int Deadline;
        public DeadlineRate DeadlineRate;
    }
}
