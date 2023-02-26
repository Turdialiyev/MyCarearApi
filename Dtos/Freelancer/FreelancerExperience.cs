namespace MyCarearApi.Dtos;

public class FreelancerExperience
{
    public string? CompanyName { get; set; }
    public string? Job { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool CurrentWorking { get; set; } = false;
    public string? Descripeion { get; set; }
}