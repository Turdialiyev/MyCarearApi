﻿namespace MyCareerApi.Entities;

public class CompanyLocations
 { 
   public int Id { get; set; }
   public string? Location { get; set; }

   public int CompanyId { get; set; }
   public Company? Company { get; set; }
 }

