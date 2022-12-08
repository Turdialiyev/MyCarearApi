namespace MyCarearApi.Models;

public class Address 
{
    public int Id { get; set; }
    public string? Country { get; set; }
    public string? Region { get; set; }
    public string? Home { get; set; }
    public int FrelanceInformationId { get; set; }
    public FreelancerInformation? FreelancerInformation { get; set; }
}