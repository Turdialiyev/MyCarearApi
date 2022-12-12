namespace MyCarearApi.Models;

public class FreelancerHobby
{
    public int Id { get; set; }
    public int HobbyId { get; set; }
    public FreelancerHobby? Hobby { get; set;}
    public int FreelancerInformationId { get; set; }
    public FreelancerInformation? FreelancerInformation { get; set; }
}