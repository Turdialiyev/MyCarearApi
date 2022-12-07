namespace MyCareerApi.Entities;
public class UserLanguages 
{ 
    public int Id { get; set; }
    public int Language { get; set; }
    public Level Level {get; set;}
    
    public int FrelanceInfoId {get; set;}
    public FrenaceInfo FrenaceInfo {get; set;}
}
