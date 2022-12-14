namespace MyCarearApi.Models;

public class FreelancerPosition
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public IEnumerable<FreelancerSkill>? FreelancerSkills { get; set; }
}