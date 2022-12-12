using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MyCarearApi.Entities;
using MyCarearApi.Services.Chat;
using System.Security.Claims;

namespace MyCarearApi.Hubs;

[Authorize]
public class ChatHub:Hub<IChatHub>
{
    public IMessageService _messageService;

    public ChatHub(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public void SendMessage(Message message)
    {
        _messageService.AddMessage(message);
        Clients.User(message.ToId).RecieveMessage(message);
    }

    public override Task OnConnectedAsync()
    {
        Clients.Client(Context.ConnectionId)
            .InitializeCHats(_messageService.GetChats(Context.User.FindFirstValue(ClaimTypes.NameIdentifier)));
        return base.OnConnectedAsync();

    }
}
