namespace MyCarearApi.Dtos;

public class FreelancerPortfolio 
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public double? Price { get; set; }
    public IFormFile? Image { get; set; }
    public int PositionId { get; set; }
}