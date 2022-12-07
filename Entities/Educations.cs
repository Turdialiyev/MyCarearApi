using MyCarearApi.Entities.Enums;

namespace MyCareerApi.Entities;
public class Educations
 {
   public int Id { get; set; }

   public string SchoolName { get; set; }
   public EducationDegree EducationDegree { get; set; }

   public TypeStudy TypeStudy { get; set; }
   public string? Location { get; set; }
   public DateTime StartDate { get; set; }
   public DateTime EndDate { get; set; }
   public bool CurrentStudy { get; set; } = false;

   public int FrelancerInfoId { get; set; }

   public FrelaceInfo FrelaceInfo { get; set; }

 }

