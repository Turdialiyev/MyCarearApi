namespace MyCarearApi.Entities;

public class FreelancerPortfolio 
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Available { get; set; }
    public string? ImageName { get; set; }
    public double? Price { get; set; }
    public int PositionId { get; set; }
    public Position? Position { get; set; }
    public string? AppUserId { get; set; }
}