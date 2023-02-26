using System.ComponentModel.DataAnnotations;

namespace MyCarearApi.Dtos;

public class Freelancer 
{
    [Required]
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public DateTime? BirthDay { get; set; }
    public IFormFile? Image { get; set; }
}