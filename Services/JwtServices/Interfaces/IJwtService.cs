using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MyCarearApi.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyCarearApi.Services.JwtServices.Interfaces
{
    public interface IJwtService
    {
        Task<string> GenerateToken(AppUser user);

        public Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(string provider, string idToken);
    }
}
