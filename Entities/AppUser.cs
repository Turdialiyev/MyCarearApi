using Microsoft.AspNetCore.Identity;

namespace MyCarearApi.Entities;

public class AppUser: IdentityUser { 
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public string? CopmanyEmail{get; set;}
   
}
