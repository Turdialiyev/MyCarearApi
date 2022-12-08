namespace MyCarearApi.Dtos;

public class ReturnCreatedCompanyLocation
{
   public int Id { get; set; }
   public string? Location { get; set; }
   public int CompanyId { get; set; }
}