﻿namespace MyCareerApi.Entities; 

public class Resume
 {
   public int Id { get; set; }
   public TypeResume TypeResume { get; set; }

   public int FrelanceInfoId { get; set; }
   public FrelaceInfo? FrenaceInfo { get; set; }
 }

