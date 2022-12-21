namespace MyCarearApi.Dtos;

public class FreelancerProject
{
    public IFormFile? Project { get; set; }
    public List<IFormFile>? ProjectImages { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Tools { get; set; }
    public string? Link { get; set; }
}