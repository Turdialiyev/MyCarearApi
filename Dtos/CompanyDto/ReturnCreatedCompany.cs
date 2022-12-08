namespace MyCarearApi.Dtos;

public class ReturnCreatedCompany
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Description { get; set; }
    public string? Photo { get; set; }
    public int ContactId { get; set; }
}