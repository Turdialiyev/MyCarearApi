namespace MyCarearApi.Entities;

public class FreelancerSkill 
{
    public int Id { get; set; }
    public int SkillId { get; set; }
    public Skill? Skill { get; set; }
    public int FrelanceInformationId { get; set; }
    public FreelancerInformation? FreelancerInformation { get; set;}
}
