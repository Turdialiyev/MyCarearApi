using MyCarearApi.Entities.Enums;

namespace MyCarearApi.Models;
public class UserLanguage
{ 
    public int Id { get; set; }
    public LangLevel? Level {get; set;}
    public int? LanguageId { get; set; }
    public string? Name { get; set; }
    public Language? Language { get; set; }
}
