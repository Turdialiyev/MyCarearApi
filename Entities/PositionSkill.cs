namespace MyCarearApi.Entities;

public class PositionSkill
{
    public int Id { get; set; }
    public int PositionsId { get; set; }
    public Position? Position { get; set; }
    public int SkillsId { get; set; }
}
