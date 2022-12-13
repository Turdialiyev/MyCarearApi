namespace MyCarearApi.Models;

public class FreelancerSkill 
{
    public int Id { get; set; }
    public int? SkillId { get; set; }
    public string? Name { get; set; }
    public int? PossitionId { get; set; }
    public Skill? Skill { get; set;}
}
