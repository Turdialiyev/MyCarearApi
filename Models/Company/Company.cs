namespace MyCarearApi.Models;

public class Company
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Photo { get; set; }
    public string AppUserId {get; set;}
    public AppUser AppUser{get; set;}

    public List<Job>? Jobs { get; set; }
    public virtual List<CompanyLocation>? CompanyLocations { get; set; }
}