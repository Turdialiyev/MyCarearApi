namespace MyCarearApi.Models;

public class Position
{
    public int FreelancerId { get; set; }
    public string? Description { get; set; }
    public DateOnly? Birthday { get; set; } = null;
    public int? PositionId { get; set; }
    public IEnumerable<FreelancerSkill>? PositionSkills { get; set; }
    public IEnumerable<FreelancerHobby>? FreelancerHobbies { get; set;}
    public IEnumerable<string> NewHobbies { get; set; }
    public IEnumerable<string> NewSkills { get; set; }
}