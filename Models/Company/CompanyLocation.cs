namespace MyCarearApi.Models;

public class CompanyLocation
{
   public int Id { get; set; }
   public string? Location { get; set; }
   public string? Description { get; set; }

   public string? UserId { get; set; }
   public AppUser? AppUser {get; set;}
}