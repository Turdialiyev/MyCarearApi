namespace MyCarearApi.Models;

public class CompanyLocation
{
   public List<string>? Locations { get; set; }
   public string? Description { get; set; }

   public string? AppUserId {get; set;}
   public AppUser? AppUser {get; set;}
}