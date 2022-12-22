namespace MyCarearApi.Services.JwtServices.Interfaces;

public interface IMailSender
{
    Task Send(string to, string subject, string body);
}
