namespace MyCarearApi.Entities;

public class Company
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Description { get; set; }
    public string? Photo { get; set; }
    public int ContactId { get; set; }
    public Contact? Conatct { get; set; }
}

