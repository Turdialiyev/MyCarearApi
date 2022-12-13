namespace MyCarearApi.Models;

public class Address 
{
    public int Id { get; set; }
    public int? CountryId { get; set; }
    public int? RegionId { get; set; }
    public string? CountryName { get; set; }
    public string? RegionName { get; set; }
    public string? Home { get; set; }
    public int FrelancerInformationId { get; set; }
    public FreelancerInformation? FreelancerInformation { get; set; }
}