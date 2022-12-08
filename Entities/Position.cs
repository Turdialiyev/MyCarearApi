using MyCareerApi.Entities;

namespace MyCarearApi.Entities;
public class Position {
    public int Id { get; set; }
    public string? Name { get; set; }

    public IEnumerable<Skill> Skills { get; set; }
 }
