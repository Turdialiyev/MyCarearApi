namespace MyCarearApi.Models;

public class Hobby
{
    public int Id { get; set; }
    public string? Hobbies { get; set; }
    public int FreelanceInformationId { get; set; }
    public FreelancerInformation? FreelancerInformation { get; set; }
}
