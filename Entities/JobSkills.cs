namespace MyCareerApi.Entities;

public class JobSkills 
{
    public int Id { get; set; }
    public int JobId { get; set; }
    public Job Job { get; set; }
}


/*
 * 
 * Table JobSkills{
  Id int [PK]
  JobId int [ref: > Job.Id]
  SkillId int [ref: > Skills.Id]
}
 * 
 * 
 * 
 * */