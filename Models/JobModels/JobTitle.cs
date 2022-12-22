# pragma warning disable
using System.ComponentModel.DataAnnotations;

namespace MyCarearApi.Models;

public class JobTitle
{
    [Required]
    public string Title { get; set; }
    [Required]
    public int PositionId { get; set; } 
}
