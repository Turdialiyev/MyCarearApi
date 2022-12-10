namespace MyCarearApi.Dtos;

public class CreateCompanyLocation
{
    public int CompnayId { get; set; }
    
    public List<CompanyLocationDto>? Locations { get; set; }
    public string? Description { get; set; }
}