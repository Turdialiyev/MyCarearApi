namespace MyCarearApi.Models;

public class Position
{
    public string? Description { get; set; }
    public DateOnly Birthday { get; set; }
    public int PositionId { get; set; }
    public int[]? PositionSkills { get; set; }
    public int[]? Hobbies { get; set; }
}