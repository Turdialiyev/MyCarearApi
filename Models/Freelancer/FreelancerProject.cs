namespace MyCarearApi.Models;

public class FreelancerProject
{
    public int Id { get; set; }
    public string? Project { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Tools { get; set; }
    public string? Link { get; set; }
    public IEnumerable<ProjectImage>? ProjectImages { get; set; }
    public DateOnly CreatedAt { get; set; }
}