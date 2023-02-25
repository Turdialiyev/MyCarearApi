namespace MyCarearApi.Dtos;

public class Position
{
    public string? Description { get; set; }

    public int? PositionId { get; set; }
    
    public int[]? FreelancerHobbies { get; set; }
    
    public int[]? FreelancerSkills { get; set; }

    public string[]? NewHobbies { get; set; }

    public string[]? NewSkills { get; set; }
}