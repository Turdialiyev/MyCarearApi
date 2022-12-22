using MyCarearApi.Services.JwtServices.Interfaces;
using Newtonsoft.Json.Linq;
using System.Net.Mail;
using System.Net;
using System.Text.Json;
using System.Text;

namespace MyCarearApi.Services.JwtServices;

public class MailSender: IMailSender
{
    private IConfiguration _configuration;

    public MailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task Send(string to, string subject, string body)
    {
        string fromMailRu = "sultonxonqudratov@mail.ru";
        string appPasswordMailRu = "ZLyCVxYmnTxUVkNBThZn";
        SmtpClient client = new SmtpClient
        {
            Host = "smtp.mail.ru",
            Port = 587,
            EnableSsl = true,
            UseDefaultCredentials = false
        };
        client.Credentials = new NetworkCredential(fromMailRu, appPasswordMailRu);

        var message = new MailMessage(new MailAddress(fromMailRu, "MyCareer"), new MailAddress(to));

        var cancellationTokenSource = new CancellationTokenSource();
        message.Subject = subject;
        message.Body = body;
        message.IsBodyHtml= true;
        Console.WriteLine("Sending");
        client.SendCompleted += (x, y) =>
        {
            Console.WriteLine(x.GetType().Name);
            Console.WriteLine(JsonSerializer.Serialize(x));
            Console.WriteLine(y.GetType().Name);
            Console.WriteLine(JsonSerializer.Serialize(x));
            cancellationTokenSource.Cancel();
        };
        client.SendMailAsync(message, cancellationTokenSource.Token);
        return Task.CompletedTask;
    }
}
