namespace MyCareerApi.Entities; 

public class Experiences
 {
  public int Id { get; set; }
  public string? CompanyName { get; set; }
  public string? Job { get; set; }
  public bool CurrentWorking { get; set; } = false;
  public DateTime StartDate { get; set; }
  public DateTime EndDate { get; set; }
  public string? Descripeion { get; set; }

  public int FrelancerInfoId { get; set; }

  public FrelaceInfo FrelaceInfo { get; set; }

 }

