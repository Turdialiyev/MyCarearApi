#pragma warning disable

using System.Reflection.Metadata;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MyCarearApi.Entities;
using MyCarearApi.Services.Chat;
using System.Security.Claims;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using MyCarearApi.Entities.Enums;

namespace MyCarearApi.Hubs;

[Authorize]
public class ChatHub:Hub<IChatHub>, IHubBase
{
    static Dictionary<string, List<string>> Users { get; set; } = new Dictionary<string, List<string>>();

    public IMessageService _messageService;
    private readonly UserManager<AppUser> _userManager;

    public ChatHub(IMessageService messageService, UserManager<AppUser> userManager)
    {
        _messageService = messageService;
        _userManager = userManager;
    }

    public async Task SendMessage(Message message)
    {
        message.FromId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        Console.WriteLine("*****************");
        Console.WriteLine(JsonSerializer.Serialize(message));
        _messageService.AddMessage(message);
        await Clients.User(message.ToId).RecieveMessage(message);
    }

    public async Task SearchUsers(string key)
    {
        await Clients.Client(Context.User.FindFirstValue(ClaimTypes.NameIdentifier)).SearchUser(_messageService.SearchUsers(key, Users));
    }

    public async Task ReadMessage(int id)
    {
        var message = await _messageService.ReadMessage(id);
        await Clients.User(message.FromId).ReadMessage(message);
    }

    public override async Task OnConnectedAsync()
    {
        Console.WriteLine("ConnectedUserId: "+Context.UserIdentifier);
        await base.OnConnectedAsync();
        var currentUserId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        string currentConnectionId = Context.ConnectionId;
        if(Users.ContainsKey(currentUserId)) Users[currentUserId].Add(currentConnectionId);
        else Users.Add(currentUserId, new List<string> { currentConnectionId });

        var chats = _messageService.GetChatsByUserInformations(currentUserId, Users);
        await Clients.Client(Context.ConnectionId).InitializeChats(chats);

        await Clients.Users(_messageService.GetChats(currentUserId)
        .Select(x => x.Member1 == currentUserId? x.Member2 : x.Member1).ToList())
        .IsOnlineChanged(new
        {
            UserId = currentUserId,
            IsOnline = true
        });
        
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine("Disconnected");
        var currentUserId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        string currentConnectionId = Context.ConnectionId;
        Users[currentUserId].Remove(currentConnectionId);

        if (!Users.ContainsKey(currentUserId) || Users[currentUserId].Count == 0)
        {
            var currentUser = await _userManager.FindByIdAsync(currentUserId);
            Clients.Users(_messageService.GetChats(currentUserId)
            .Select(x => x.Member1 == currentUserId ? x.Member2 : x.Member1).ToList())
            .IsOnlineChanged(new
            {
                Id = currentUserId,
                currentUser.UserName,
                currentUser.FirstName,
                currentUser.LastName,
                currentUser.Email,
                currentUser.PhoneNumber,
                IsOnline = false
            });
        }

       // Console.WriteLine(exception.Message);
        base.OnDisconnectedAsync(exception);
    }
}
