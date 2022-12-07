using MyCarearApi.Entities.Enums;

namespace MyCareerApi.Entities;
public class UserLanguage
{ 
    public int Id { get; set; }
    public int Language { get; set; }
    public Level Level {get; set;}
    
    public int FrelanceInfoId {get; set;}

    public FrelaceInfo FrelaceInfo {get; set;}
}
