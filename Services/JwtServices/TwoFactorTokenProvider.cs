using Microsoft.AspNetCore.Identity;
using MyCarearApi.Entities;

namespace MyCarearApi.Services.JwtServices;

public class TwoFactorTokenProvider<T> : IUserTwoFactorTokenProvider<T> where T: IdentityUser 
{
    private static Dictionary<string, string> Tokens { get; set; } = new Dictionary<string, string>();

    public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<T> manager, T user) => Task.FromResult(true);

    public Task<string> GenerateAsync(string purpose, UserManager<T> manager, T user)
    {
        var token = Guid.NewGuid().ToString() + Guid.NewGuid().ToString();
        Tokens.Add(user.Id, token);
        new Task(() => { Thread.Sleep(TimeSpan.FromHours(24)); if (Tokens.ContainsKey(user.Id)) Tokens.Remove(user.Id); });
        return Task.FromResult(token);
    }

    public Task<bool> ValidateAsync(string purpose, string token, UserManager<T> manager, T user)
    {
        if (Tokens.ContainsKey(user.Id) && Tokens[user.Id] == token)
        {
            Tokens.Remove(user.Id);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }
}
