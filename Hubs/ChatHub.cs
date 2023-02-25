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

    public async Task WriteMessage(Message message, List<string> filePaths)
    {
        message.FromId = Context.UserIdentifier;
        var msg = _messageService.AddMessage(message, filePaths);
        var messageToClient = await MessageToReturn(msg);
        
        var connectionIds = _connectionService.GetConnections(message.ToId);
        connectionIds.ForEach(async x =>
        {
            var proxy = Clients.Client(x);
            await proxy.RecieveMessage(messageToClient);
        });
    }

    public async Task DeleteMessage(int messageId)
    {
        var message = _messageService.GetMessage(messageId);
        if (message is null || (message.FromId != Context.UserIdentifier && message.ToId != Context.UserIdentifier)) return;
        _messageService.RemoveMessage(messageId);
        await Clients.Users(message.FromId, message.ToId)
            .MessageRemoved(new {MessageId = message.Id, message.ChatId, RemovedBy = Context.UserIdentifier});
    }

    public async Task ClearHistory(int chatId)
    {
        var chat = _messageService.GetChat(chatId);
        if (chat is null || (chat.Member1 != Context.UserIdentifier && chat.Member2 != Context.UserIdentifier)) return;
        _messageService.ClearHistory(chatId);
        await Clients.Users(chat.Member1, chat.Member2).HistoryCleared(new {ChatId = chatId, ClearedBy = Context.UserIdentifier });
    }

    public async IAsyncEnumerable<char> GetChat(int id)
    {
        var chat = await _messageService.GetChat(id, Users);
        var jsonChat = JsonSerializer.Serialize(chat);
        foreach (var b in jsonChat)
        {
            yield return b;
        }
    }

    public async IAsyncEnumerable<char> SearchUsers(string key, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var users = JsonSerializer.Serialize(_messageService.SearchUsers(key, Users, Context.UserIdentifier));
        foreach (var b in users)
        {
            yield return b;
        } 
    }

    public async Task ReadMessage(int id)
    {
        var message = await _messageService.ReadMessage(id);
        var connections = _connectionService.GetConnections(message.FromId);
        connections.AddRange(_connectionService.GetConnections(message.ToId));
        var proxy = Clients.Clients(connections);
        await proxy.ReadMessage(new
        {
            message.Id,
            message.FromId,
            message.ToId,
            message.DateTime,
            message.Text,
            message.IsRead,
            message.ChatId,
            message.ChatFiles,
            message.HasFile,
            message.HasLink,
            message.HasMedia,
            Chat = new
            {
                message.Chat.Id,
                Member1 = AppUserToReturn(await _userManager.FindByIdAsync(message.Chat.Member1)),
                Member2 = AppUserToReturn(await _userManager.FindByIdAsync(message.Chat.Member2)),
                message.Chat.DateTime,
                Messages = new List<string>()
            }
        });
    }

    public async IAsyncEnumerable<char> GetHistory([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        foreach (var chat in JsonSerializer.Serialize(_messageService.GetChatsByUserInformations(Context.UserIdentifier, Users)))
        {
            yield return chat;
        }
    }

    public async Task RemoveFilesFromMessage(List<int> fileIds, int messageId)
    {
        await _messageService.RemoveFiles(fileIds);
        var message = _messageService.GetMessage(messageId);
        Clients.Users(message.FromId, message.ToId).MessageEdited(new
        {
            EditedBy = Context.UserIdentifier,
            Message = await MessageToReturn(message)
        });
    }

    public async Task AddFilesToMessage(List<string> paths, int messsageId)
    {
        var addedFiles = _messageService.SaveFiles(messsageId, paths);
        var message = _messageService.GetMessage(messsageId);
        Clients.Users(message.FromId, message.ToId).MessageEdited(new
        {
            EditedBy = Context.UserIdentifier,
            Message = await MessageToReturn(message)
        });
    }

    private async Task<dynamic> MessageToReturn(Message message) => new
    {
        message.Id,
        message.FromId,
        message.ToId,
        message.DateTime,
        message.Text,
        message.IsRead,
        message.ChatId,
        ChatFiles = message.ChatFiles.Select(x => new
        {
            x.Id,
            x.Path,
            x.MessageId,
            x.MediType
        }),
        Chat = new
        {
            message.Chat.Id,
            Member1 = await _userManager.FindByIdAsync(message.Chat.Member1),
            Member2 = await _userManager.FindByIdAsync(message.Chat.Member2),
            message.Chat.DateTime,
            Messages = new List<string>()
        }
    };

    private async Task<dynamic> AppUserToReturn(AppUser user) => new
    {
        user.Id,
        user.Email,
        user.FirstName,
        user.LastName,
        user.PhoneNumber
    };

    public async Task UpdateMessageText(string text, int messageId)
    {
        var message = await _messageService.UpdateText(text, messageId);
        Clients.Users(message.FromId, message.ToId).MessageEdited(new
        {
            EditedBy = Context.UserIdentifier,
            Message = await MessageToReturn(message)
        });
    }

    public override async Task OnConnectedAsync()
    {
        base.OnConnectedAsync();
        _connectionService.AddConnection(Context.ConnectionId, Context.UserIdentifier);

        Clients.Caller.InitializeChats();

        var connectionIds = new List<string>();
        _connectionService.GetUserIds().Where(x => x!= Context.UserIdentifier).ToList().ForEach(x => connectionIds.AddRange(_connectionService.GetConnections(x)));
        if(!connectionIds.Any() ) { return; }
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
        
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _connectionService.RemoveConnection(Context.ConnectionId);

        var connectionIds = new List<string>();
        _connectionService.GetUserIds().ForEach(x => connectionIds.AddRange(_connectionService.GetConnections(x)));
        if (!connectionIds.Any()) return;
        var proxy = Clients.Clients(connectionIds);
        if (proxy is null) return;
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