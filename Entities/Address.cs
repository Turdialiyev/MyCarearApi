namespace MyCarearApi.Entities;

public class Address 
{
    public int Id { get; set; }

    public int? CountryId { get; set; }

    public int? RegionId { get; set; }

    public string? Home { get; set; }

    public int FrelancerInformationId { get; set; }

    public FreelancerInformation? FreelancerInformation { get; set; }
}



/*
 * 
 * 
Table Address{
  Id int [PK]
  Country nvarchar
  Region nvarchar
  Home nvarchar
  FrenaceInfoId nvarchar [ref: > FrenaceInfo.Id]
}
 * */