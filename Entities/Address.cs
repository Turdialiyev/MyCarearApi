namespace MyCarearApi.Entities;

public class Address
{
    public int Id { get; set; }

    public int? CountryId { get; set; }
    public Country? Country { get; set; }
    public int? RegionId { get; set; }
    public Region? Region { get; set; }
    public string? Home { get; set; }

    public int? FrelancerInformationId { get; set; }

    public FreelancerInformation? FreelancerInformation { get; set; }
}