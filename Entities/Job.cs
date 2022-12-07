namespace MyCareerApi.Entities; 

public class Job 
{
    public int Id { get; set; }
    public decimal Price { get; set; }

    public JobState State { get; set; }

    public int PositionsId { get; set; }

    public Positions Positions { get; set; }


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