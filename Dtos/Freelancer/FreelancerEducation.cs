using MyCarearApi.Entities.Enums;

namespace MyCarearApi.Dtos;

public class FreelancerEducation
{
    public string? SchoolName { get; set; }
    public string? EducationDegree { get; set; }
    public string? TypeStudy { get; set; }
    public string? Location { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public bool CurrentStudy { get; set; } = false;
}