﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyCarearApi.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyCarearApi.Services.JwtServices;

public class JwtService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public JwtService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
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

}
