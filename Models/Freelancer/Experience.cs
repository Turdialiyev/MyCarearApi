namespace MyCarearApi.Models;

public class Experience
 {
  public int Id { get; set; }
  public string? CompanyName { get; set; } 
  public string? Job { get; set; }
  public bool CurrentWorking { get; set; } = false;
  public DateOnly StartDate { get; set; }
  public DateOnly EndDate { get; set; }
  public string? Descripeion { get; set; }

  public int FrelancerInformationId { get; set; }
  public FreelancerInformation? FreelancerInformation { get; set; }

 }

