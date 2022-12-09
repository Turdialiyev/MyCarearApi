namespace MyCarearApi.Models;
public class Contact 
{ 
    public int Id { get; set; }
    public string?  WatsApp { get; set; }
    public string?  Facebook { get; set; }
    public string?  Telegram { get; set; }
    public string?  GitHub { get; set; }
    public string?  Twitter { get; set; }
    public string?  Instagram { get; set; }
    public string?  WebSite { get; set; }

    public string? UserId { get; set; }
    public AppUser? AppUser {get; set;}
}
