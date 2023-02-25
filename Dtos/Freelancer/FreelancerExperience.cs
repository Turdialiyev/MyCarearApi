namespace MyCarearApi.Dtos;

public class FreelancerExperience
{
    public string? CompanyName { get; set; }
    public string? Job { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public bool CurrentWorking { get; set; } = false;
    public string? Descripeion { get; set; }
}