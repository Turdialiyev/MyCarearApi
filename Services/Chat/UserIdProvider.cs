# pragma warning disable
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace MyCarearApi.Services.Chat;

public class UserIdProvider: IUserIdProvider
{
    public string GetUserId(HubConnectionContext context)
    {
        return context.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
