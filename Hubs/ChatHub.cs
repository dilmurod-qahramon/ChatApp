using ChatApp.Models;
using ChatApp.Services.interfaces;
using Microsoft.AspNetCore.SignalR;  

namespace ChatApp.Hubs;

public class ChatHub(IChatService chatService, IUserService userService, IMessageService messageService) : Hub
{
    public async Task SendMessage(Guid chatId, Guid userId, string message)
    {
        var chat = await chatService.GetChatByIdAsync(chatId)
           ?? throw new HubException("Chat not found.");
        var user = await userService.GetUserByIdAsync(userId)
           ?? throw new Exception("User is not found!");
        if (await chatService.IsMemberOfChat(userId, chatId))
            throw new HubException("User is not a member of this chat.");
        if (message.Trim().Length == 0) return;
        var chatMessage = new Message
        {
            Id = Guid.NewGuid(),
            ChatId = chatId,
            UserId = userId,
            Text = message,
        };

        await messageService.CreateNewMessageAsync(chatMessage);
        await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", userId, message);
    }

    public async Task JoinChat(Guid chatId, Guid userId)
    {
        var user = await userService.GetUserByIdAsync(userId)
            ?? throw new Exception("User is not found!");
        var chat = await chatService.GetChatByIdAsync(chatId)
            ?? throw new Exception("Chat is not found!");
        if (await chatService.IsMemberOfChat(userId, chatId)) return;

        await chatService.AddNewUserToChatAsync(chatId, userId);
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
    }

    public async Task LeaveChat(Guid chatId, Guid userId)
    {
        var chat = await chatService.GetChatByIdAsync(chatId)
            ?? throw new HubException("Chat is not found!");
        var user = await userService.GetUserByIdAsync(userId)
            ?? throw new Exception("User is not found!");
        if (await chatService.IsMemberOfChat(userId, chatId))
            await chatService.RemoveUserFromChatAsync(chatId, userId);

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
    }

    public async Task ClearChatHistory(Guid chatId)
    {
        var chat = await chatService.GetChatByIdAsync(chatId)
           ?? throw new HubException("Chat is not found!");
        if (chat.Messages.Count() > 0)
            throw new HubException("Chat history is already empty.");

        await chatService.ClearChatHistoryAsync(chatId);
        await Clients.Group(chatId.ToString()).SendAsync("ChatHistoryCleared", chatId);
    }

    public async Task AddUserToChat(Guid chatId, Guid userId)
    {
        var user = await userService.GetUserByIdAsync(userId)
        ?? throw new HubException("User not found.");

        var chat = await chatService.GetChatByIdAsync(chatId)
            ?? throw new HubException("Chat not found.");

        await chatService.AddNewUserToChatAsync(chatId, userId);
        await Clients.Group(chatId.ToString()).SendAsync("UserAdded", chatId, userId);
    }

    public async Task RemoveUserFromChat(Guid chatId, Guid userId, Guid ownerId)
    {
        var user = await userService.GetUserByIdAsync(userId)
            ?? throw new HubException("User not found.");

        var chat = await chatService.GetChatByIdAsync(chatId)
            ?? throw new HubException("Chat not found.");
        if (chat.OwnerId != ownerId)
        {
            throw new HubException("Can't remove a user");
        }
        if (!await chatService.IsMemberOfChat(userId, chatId))
        {
            throw new HubException("User is not a member of this chat.");
        }
        await chatService.RemoveUserFromChatAsync(chatId, userId);
        await Clients.Group(chatId.ToString()).SendAsync("UserRemoved", chatId, userId);
    }

    public async Task DeleteChat(Guid chatId, Guid requestingUserId)
    {
        var chat = await chatService.GetChatByIdAsync(chatId)
        ?? throw new HubException("Chat not found.");

        if (chat.OwnerId != requestingUserId)
        {
            throw new HubException("You do not have permission to delete this chat.");
        }

        await chatService.DeleteChatAsync(chatId);
        await Clients.Group(chatId.ToString()).SendAsync("ChatDeleted", chatId);
    }

    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"✅ Client connected: {Context.ConnectionId}");
        await base.OnConnectedAsync();
    }
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine($"Client disconnected. Error: {exception?.Message}");
        await base.OnDisconnectedAsync(exception);
    }
}
