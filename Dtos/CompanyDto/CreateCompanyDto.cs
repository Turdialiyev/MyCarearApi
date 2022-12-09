namespace MyCarearApi.Dtos;

public class CreateCompanyDto
{
    public string? Name { get; set; }
    public string? PhoneNumber { get; set; }

    public IFormFile? File { get; set; }
}