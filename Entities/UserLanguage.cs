using MyCarearApi.Entities.Enums;

namespace MyCarearApi.Entities;
public class UserLanguage
{ 
    public int Id { get; set; }
    public int? LanguageId { get; set; }
    public Language? Language { get; set; }
    public string? Level {get; set;}
    public int FreelancerInformationId {get; set;}
    public FreelancerInformation? FreelancerInformation {get; set;}
}
