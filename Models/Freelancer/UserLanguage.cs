using MyCarearApi.Entities.Enums;

namespace MyCarearApi.Models;
public class UserLanguage
{ 
    public int Id { get; set; }
    public int LanguageId { get; set; }
    public LangLevel Level {get; set;}
    public int FrelanceInformationId {get; set;}
    public FreelancerInformation? FreelancerInformation {get; set;}
}
