# pragma warning disable
using System.ComponentModel.DataAnnotations;

namespace MyCarearApi.Models;

public class JobTitle
{
    public int JobId { get; set; }

    [Required]
    public string Title { get; set; }
    [Required]
    public int PositionId { get; set; } 
}
