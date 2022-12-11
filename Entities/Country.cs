namespace MyCarearApi.Entities;

public class Country
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public IEnumerable<Region>? Regions { get; set; }
}