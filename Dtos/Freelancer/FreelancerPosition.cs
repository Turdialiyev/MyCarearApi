namespace MyCarearApi.Dtos;

public class Position
{
    public string? Description { get; set; }
    public DateOnly Birthday { get; set; }
    public int PositionId { get; set; }
    public List<FreelancerHobby>? FreelancerHobbies{ get; set; }
    public List<FreelancerSkill>? FreelancerSkills { get; set; }
}