using MyCarearApi.Entities.Enums;

namespace MyCarearApi.Entities;
public class UserLanguage
{ 
    public int Id { get; set; }
    public int Language { get; set; }
    public Level Level {get; set;}
    public int FrelanceInfoId {get; set;}
    public FreelancerInformation? FreelancerInformation {get; set;}
}
