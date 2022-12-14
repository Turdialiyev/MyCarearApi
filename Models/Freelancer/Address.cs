namespace MyCarearApi.Models;

public class Address
{
    public int Id { get; set; } = 0;
    public int? CountryId { get; set; } = 0;
    public int? RegionId { get; set; } = 0;
    public string? CountryName { get; set; } = null;
    public string? RegionName { get; set; } = null;
    public string? Home { get; set; } = null;
    public int? FrelancerInformationId { get; set; } = 0;
    public FreelancerInformation? FreelancerInformation { get; set; } = null;
}