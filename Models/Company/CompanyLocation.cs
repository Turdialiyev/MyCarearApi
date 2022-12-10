namespace MyCarearApi.Models;

public class CompanyLocation
{
   public int Id { get; set; }
   public string? Location { get; set; }
   public string? Description { get; set; }

   public int CompnayId { get; set; }
   public Company? Company { get; set; }
}