using MyCareerApi.Entities;

namespace MyCarearApi.Entities;

public class Job 
{
    public int Id { get; set; }

    public  string Name { get; set; }
    public string Description { get; set; }
    public string FilePath { get; set; }
    public decimal Price { get; set; }

    public JobState State { get; set; }

    public int DeadLine { get; set; }

    //Category
    public int PositionsId { get; set; }

    public Position Position { get; set; }

    public int CompanyId { get; set; }
    public Company Company { get; set; }

    public IEnumerable<JobSkill> JobSkills { get; set; }
}


/*
 * 
 * 
Table Job{
  Id int [PK]
  Price money
  State JobState
  PositionsId nvarchar [ref: > Positions.Id]
}
 * 
 * */