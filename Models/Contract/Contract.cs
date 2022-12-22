# pragma warning disable
using MyCarearApi.Entities.Enums;

namespace MyCarearApi.Models;

public class Contract
{
    public int Id { get; set; }
    public string PasportSeriyaNumber { get; set; }
    public string INN { get; set; }
    public string BankName { get; set; }
    public string TranzitAccount { get; set; }
    public string CardNumber { get; set; }
    public string INPS { get; set; }
    public string BankINN { get; set; }
    public string MFO { get; set; }
    public ContractState State { get; set; }
    public DateOnly? DealingDate { get; set; }

    public int JobId { get; set; }
    public Job Job { get; set; }

    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }
}