using ChatApp.Models;
using ChatApp.Services.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;  

namespace ChatApp.Hubs;
[Authorize]
public class ChatHub(IChatService chatService, IMessageService messageService) : Hub
{
    private static readonly Dictionary<Guid, List<string>> _userConnections = [];
    public async Task SendMessage(Guid chatId, string message)
    {
        var userIdStr = (Context.User?.FindFirst("sub")?.Value)
         ?? throw new HubException("User not found.");
        var userId = Guid.Parse(userIdStr);

        var chat = await chatService.GetChatByIdAsync(chatId)
           ?? throw new HubException("Chat not found.");
      
        if (!await chatService.IsMemberOfChat(userId, chatId))
            throw new HubException("User is not a member of this chat.");
        if (message.Trim().Length == 0) return;
        var chatMessage = new Message
        {
            Id = Guid.NewGuid(),
            ChatId = chatId,
            UserId = userId,
            Text = message,
        };

        var newChatMessage = await messageService.CreateNewMessageAsync(chatMessage);
        await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", newChatMessage);
    }

    public async Task JoinChat(Guid chatId)
    {
        var userIdStr = (Context.User?.FindFirst("sub")?.Value)
          ?? throw new HubException("User not found.");
        var userId = Guid.Parse(userIdStr);

        var chat = await chatService.GetChatByIdAsync(chatId)
            ?? throw new Exception("Chat is not found!");

        if (!await chatService.IsMemberOfChat(userId, chatId))
            await chatService.AddNewUserToChatAsync(chatId, userId);

        if (!_userConnections.ContainsKey(userId))
            _userConnections[userId] = [];

        if (!_userConnections[userId].Contains(Context.ConnectionId))
        {
            _userConnections[userId].Add(Context.ConnectionId);
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
        }
    }

    public async Task LeaveChat(Guid chatId)
    {
        var userIdStr = (Context.User?.FindFirst("sub")?.Value)
          ?? throw new HubException("User not found.");
        var userId = Guid.Parse(userIdStr);
        var chat = await chatService.GetChatByIdAsync(chatId)
            ?? throw new HubException("Chat is not found!");
       
        if (!await chatService.IsMemberOfChat(userId, chatId))
            return;
        await chatService.RemoveUserFromChatAsync(chatId, userId);

        if (_userConnections.ContainsKey(userId))
        {
            foreach (var connectionId in _userConnections[userId])
            {
                await Groups.RemoveFromGroupAsync(connectionId, chatId.ToString());
            }

            _userConnections.Remove(userId);
        }
        await Clients.Group(chatId.ToString()).SendAsync("UserLeft", chatId, userId);
    }

    public async Task ClearChatHistory(Guid chatId)
    {
        var userIdStr = (Context.User?.FindFirst("sub")?.Value)
           ?? throw new HubException("User not found.");
        var userId = Guid.Parse(userIdStr);

        var chat = await chatService.GetChatByIdAsync(chatId)
           ?? throw new HubException("Chat is not found!");
        
        if (chat.OwnerId != userId) 
            throw new HubException("Only owners of the chat can clear it!");

        await chatService.ClearChatHistoryAsync(chatId);
        await Clients.Group(chatId.ToString()).SendAsync("ChatHistoryCleared", chatId);
    }

    public async Task AddUserToChat(Guid chatId)
    {
        var userIdStr = (Context.User?.FindFirst("sub")?.Value) 
            ?? throw new HubException("User not found.");
        var userId = Guid.Parse(userIdStr);
        var chat = await chatService.GetChatByIdAsync(chatId)
            ?? throw new HubException("Chat not found.");

        await chatService.AddNewUserToChatAsync(chatId, userId);
        if (!_userConnections.ContainsKey(userId))
            _userConnections[userId] = [];

        if (!_userConnections[userId].Contains(Context.ConnectionId))
        {
            _userConnections[userId].Add(Context.ConnectionId);
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
        }

        await Clients.Group(chatId.ToString()).SendAsync("UserAdded", chatId, userId);
    }

    public async Task RemoveUserFromChat(Guid chatId, Guid userId)
    {
        var requestingUserId = Context.User?.FindFirst("sub")?.Value;

        var chat = await chatService.GetChatByIdAsync(chatId)
            ?? throw new HubException("Chat not found.");
        if (chat.OwnerId.ToString() != requestingUserId)
        {
            throw new HubException("Can't remove a user");
        }
        if (await chatService.IsMemberOfChat(userId, chatId))
            return;

        await chatService.RemoveUserFromChatAsync(chatId, userId);
        if (_userConnections.ContainsKey(userId))
        {
            foreach (var connectionId in _userConnections[userId])
            {
                await Groups.RemoveFromGroupAsync(connectionId, chatId.ToString());
            }

            _userConnections.Remove(userId);
        }

        await Clients.Group(chatId.ToString()).SendAsync("UserRemoved", chatId, userId);
    }

    public override async Task OnConnectedAsync()
    {
        var userIdStr = (Context.User?.FindFirst("sub")?.Value)
            ?? throw new HubException("User not found.");
        var userId = Guid.Parse(userIdStr);

        var chatIds = await chatService.GetAllUserChatsByUserIdAsync(userId); 
        if (!_userConnections.ContainsKey(userId))
            _userConnections[userId] = new List<string>();

        _userConnections[userId].Add(Context.ConnectionId);

        foreach (var chatId in chatIds)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userIdStr = (Context.User?.FindFirst("sub")?.Value)
            ?? throw new HubException("User not found.");
        var userId = Guid.Parse(userIdStr);
        if (_userConnections.TryGetValue(userId, out var connections))
        {
            connections.Remove(Context.ConnectionId);
            if (connections.Count == 0)
            {
                _userConnections.Remove(userId);
            }
        }
        await base.OnDisconnectedAsync(exception);
    }
}
