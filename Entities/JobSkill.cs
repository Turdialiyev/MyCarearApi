namespace MyCarearApi.Entities;

public class JobSkill
{
    public int Id { get; set; }
    public int JobId { get; set; }
    public Job Job { get; set; }
}