namespace MyCarearApi.Entities;

public class FreelancerInformation
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? FreelancerImage { get; set; }
    public DateOnly Birthday { get; set; }
    public string? Description { get; set; }
    public int PositionId { get; set; }
    public Position? Position { get; set; }
    public int ContactId { get; set; }
    public Contact? Contact { get; set;}
    public int AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    public int AddressId { get; set; }
    public Address? Address { get; set;}
    public IEnumerable<FreelancerHobby>? Hobbies { get; set; }
    public IEnumerable<FreelancerSkill>? FreelancerSkills { get; set;}
    public IEnumerable<Experience>? Experiences { get; set;}
    public IEnumerable<UserLanguage>? userLanguages { get; set;}
    public IEnumerable<Education>? Educations { get; set; }
}
