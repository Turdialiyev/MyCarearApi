using Microsoft.AspNetCore.SignalR;

namespace MyCarearApi.Services.Chat.Interfaces
{
    public interface IUserIdProvider
    {
        string GetUserId(HubConnectionContext context);
    }
}
