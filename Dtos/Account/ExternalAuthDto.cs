namespace MyCarearApi.Dtos.Account;

public class ExternalAuthDto
{
    public string? Provider { get; set; }
    public string? IdToken { get; set; }
}