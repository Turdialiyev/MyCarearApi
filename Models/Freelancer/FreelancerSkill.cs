namespace MyCarearApi.Models;

public class FreelancerSkill 
{
    public int Id { get; set; }
    public int? SkillId { get; set; }
    public Skill? Skill { get; set;}
}
