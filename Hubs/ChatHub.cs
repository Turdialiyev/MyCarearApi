#pragma warning disable

using System.Reflection.Metadata;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MyCarearApi.Entities;
using MyCarearApi.Services.Chat;
using System.Security.Claims;
using System.Linq;
//using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using MyCarearApi.Entities.Enums;
using MyCarearApi.Services.Chat.Interfaces;
using System.Text.Json;
using System.Text;
using System.Runtime.CompilerServices;

namespace MyCarearApi.Hubs;

[Authorize]
public class ChatHub:Hub<IChatHub>, IHubBase
{
   
    public IMessageService _messageService;
    private readonly UserManager<AppUser> _userManager;
    private readonly IConnectionService _connectionService;

    private Dictionary<string, List<string>> Users => 
        _connectionService.GetUserIds().ToDictionary(x => x, x => _connectionService.GetConnections(x));

    public ChatHub(IMessageService messageService, UserManager<AppUser> userManager, IConnectionService connectionService)
    {
        _messageService = messageService;
        _userManager = userManager;
        _connectionService = connectionService;
        
    }

    public async Task WriteMessage(Message message)
    {
        message.FromId = Context.UserIdentifier;
         var msg = _messageService.AddMessage(message);
        var messageToClient = new
        {
            msg.Id,
            msg.FromId,
            msg.ToId,
            msg.DateTime,
            msg.Text,
            msg.FileMessage,
            msg.FileName,
            msg.IsRead,
            msg.ChatId,
            Chat = new
            {
                msg.Chat.Id,
                Member1 = await _userManager.FindByIdAsync(msg.Chat.Member1),
                Member2 = await _userManager.FindByIdAsync(msg.Chat.Member2),
                msg.Chat.DateTime,
                Messages = new List<string>()
            }
        };
        var connectionIds = _connectionService.GetConnections(message.ToId);
        connectionIds.ForEach(async x =>
        {
            var proxy = Clients.Client(x);
            await proxy.RecieveMessage(messageToClient);
        });
    }

    public async IAsyncEnumerable<Chat> GetChat(int id)
    {
        var chat = await _messageService.GetChat(id, Users);
        var jsonChat = JsonSerializer.Serialize(chat);
        foreach (var b in Encoding.UTF8.GetBytes(jsonChat))
        {
            yield return b;
        }
    }

    public async IAsyncEnumerable<byte> SearchUsers(string key, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var users = JsonSerializer.Serialize(_messageService.SearchUsers(key, Users));
        foreach (var b in Encoding.UTF8.GetBytes(users))
        {
            yield return b;
        }
    }

    public async Task ReadMessage(int id)
    {
        var message = await _messageService.ReadMessage(id);
        var proxy = Clients.Clients(_connectionService.GetConnections(message.FromId));
        await proxy.ReadMessage(new
        {
            message.Id,
            message.FromId,
            message.ToId,
            message.DateTime,
            message.Text,
            message.FileMessage,
            message.FileName,
            message.IsRead,
            message.ChatId,
            Chat = new
            {
                message.Chat.Id,
                Member1 = await _userManager.FindByIdAsync(message.Chat.Member1),
                Member2 = await _userManager.FindByIdAsync(message.Chat.Member2),
                message.Chat.DateTime,
                Messages = new List<string>()
            }
        });
    }

    public async IAsyncEnumerable<byte> GetHistory([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        foreach (var chat in Encoding.UTF8.GetBytes(JsonSerializer.Serialize(_messageService.GetChatsByUserInformations(Context.UserIdentifier, Users))))
        {
            yield return chat;
        }
    }

    public override async Task OnConnectedAsync()
    {
        
        Clients.Caller.RecieveMessage(new Message { Text = "\n*****************\n************Global message\n*************\n****************" });

        Console.WriteLine("-----------------" + Context.UserIdentifier);
        _connectionService.AddConnection(Context.ConnectionId, Context.UserIdentifier);

        Clients.Caller.InitializeChats();

        var connectionIds = new List<string>();
        _connectionService.GetUserIds().Where(x => x!= Context.UserIdentifier).ToList().ForEach(x => connectionIds.AddRange(_connectionService.GetConnections(x)));
        var proxy = Clients.Clients(connectionIds);
        var user = await _userManager.FindByIdAsync(Context.UserIdentifier);
        proxy.IsOnlineChanged(new
        {
            user.Id,
            user.UserName,
            user.FirstName,
            user.LastName,
            user.Email,
            user.PhoneNumber,
            IsOnline = true
        });
        
        base.OnConnectedAsync();
    }


    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _connectionService.RemoveConnection(Context.ConnectionId);

        var connectionIds = new List<string>();
        _connectionService.GetUserIds().ForEach(x => connectionIds.AddRange(_connectionService.GetConnections(x)));
        var proxy = Clients.Clients(connectionIds);
        var user = await _userManager.FindByIdAsync(Context.UserIdentifier);
        proxy.IsOnlineChanged(new
        {
            user.Id,
            user.UserName,
            user.FirstName,
            user.LastName,
            user.Email,
            user.PhoneNumber,
            IsOnline = false 
        });

        await base.OnDisconnectedAsync(exception);
    }
}
