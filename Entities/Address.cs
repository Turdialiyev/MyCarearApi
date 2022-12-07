namespace MyCareerApi.Entities; 

public class Address 
{
    public int Id { get; set; }

    public string Country { get; set; }

    public string Region { get; set; }

    public string Home { get; set; }

    public int FrelanceInfoId { get; set; }

    public FrelaceInfo FrelanceInfo { get; set; }
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