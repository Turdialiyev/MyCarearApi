using MyCarearApi.Entities.Enums;

namespace MyCarearApi.Models;

public class FreelancerInformation
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? FreelancerImage { get; set; }
    public DateOnly? Birthday { get; set; }
    public string? Description { get; set; }
    public TypeResume? TypeResume { get; set; }
    public bool? Finish { get; set; }
    public int? PossionId { get; set; }
    public string? Position { get; set; }
    public int? FreelancerContactId { get; set; }
    public FreelancerContact? FreelancerContact { get; set; }
    public int? AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    public int? AddressId { get; set; }
    public Address Address { get; set; }
    public IEnumerable<FreelancerHobby>? FreelancerHobbies { get; set; }
    public IEnumerable<FreelancerSkill>? FreelancerSkills { get; set; }
    public IEnumerable<Experience>? Experiences { get; set; }
    public IEnumerable<UserLanguage>? UserLanguages { get; set; }
    public IEnumerable<Education>? Educations { get; set; }
}
