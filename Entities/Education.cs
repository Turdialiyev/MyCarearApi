# pragma warning disable
using MyCarearApi.Entities.Enums;

namespace MyCarearApi.Entities;
public class Education
 {
   public int Id { get; set; }

   public string? SchoolName { get; set; }
   public string? EducationDegree { get; set; }
   public string? TypeStudy { get; set; }
   public string? Location { get; set; }
   public DateOnly? StartDate { get; set; }
   public DateOnly? EndDate { get; set; }
   public bool CurrentStudy { get; set; } = false;

   public int FreelancerInformationId { get; set; }
   public FreelancerInformation FreelancerInformation { get; set; }

 }

