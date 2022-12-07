using MyCarearApi.Entities.Enums;

namespace MyCarearApi.Entities;

public class Resume
 {
   public int Id { get; set; }
   public TypeResume TypeResume { get; set; }

   public int FrelanceInfoId { get; set; }
   public FreelancerInformation FreelancerInformation { get; set; }

 }

