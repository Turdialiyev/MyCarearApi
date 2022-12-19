namespace MyCarearApi.Models;

public class Dogovor
{
    public DateOnly ContractDate {get; set;}
    public string? FreelancerName { get; set; }
    public string? PassportSeria { get; set; }
    public string? JobTitle { get; set; }
    public string? Position { get; set; }
    public string? JobDescription { get; set; }
    public string? Summa { get; set; }
    public DateOnly Diedline { get; set; }
    public Decimal AdvancePayment { get; set; }
    public Decimal LastPayment {get; set;}
}