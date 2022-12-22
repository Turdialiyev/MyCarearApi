# pragma warning disable
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyCarearApi.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MyCarearApi.Services.JwtServices.Interfaces;
using Google.Apis.Auth;

namespace MyCarearApi.Services.JwtServices;

public class JwtService: IJwtService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly IConfigurationSection _configurationGoogle;

    public JwtService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _configurationGoogle = configuration.GetSection("Google");
    }

    public async Task<string> GenerateToken(AppUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email)
        };

        await _roleManager.Roles.ForEachAsync(async x =>
        {
            if (await _userManager.IsInRoleAsync(user, x.Name))
            {
                claims.Add(new Claim(ClaimTypes.Role, x.Name));
            }
        });

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetSecretKey()));
        var signInCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var tokenOptions = new JwtSecurityToken(
            issuer: GetIssuer(),
            audience: GetAudience(),
            claims: claims,
            expires: DateTime.Now.AddMinutes(5),
            signingCredentials: signInCredentials
            );

        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        return token;
    }

    private string GetSecretKey() => _configuration.GetSection("Jwt")["JwtSecretKey"];

    private string GetIssuer() => _configuration.GetSection("Jwt")["Issuer"];

    private string GetAudience() => _configuration.GetSection("Jwt")["Audience"];

    public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(string provider, string idToken)
    {
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { _configurationGoogle.GetSection("client_id").Value }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
            return payload;
        }
        catch (Exception)
        {
            return null;
        }
    }

}
