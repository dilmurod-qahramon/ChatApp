using ChatApp.Models;
using ChatApp.Services.interfaces;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Hubs
{
    public class ChatHub(IChatService chatService, IUserService userService, IMessageService messageService) : Hub
    {
        private readonly IChatService _chatService = chatService;
        private readonly IUserService _userService = userService;
        private readonly IMessageService _messageService = messageService;

        public async Task SendMessage(Guid chatId, Guid userId, string message)
        {
            var chat = await _chatService.GetChatByIdAsync(chatId) 
                ?? throw new HubException("Chat not found.");
            var chatMessage = new Message
            {
                Id = Guid.NewGuid(),
                ChatId = chatId,
                UserId = userId,
                Text = message,
            };

            chat.Messages.Add(chatMessage);
            await _messageService.CreateNewMessageAsync(chatMessage);
            await _chatService.UpdateChatAsync(chat);

            await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", userId, message);
        }

        public async Task JoinChat(Guid chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
        }

        public async Task LeaveChat(Guid chatId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
        }

        public async Task ClearChatHistory(Guid chatId)
        {
            await _chatService.ClearChatHistoryAsync(chatId);
            await Clients.Group(chatId.ToString()).SendAsync("ChatHistoryCleared", chatId);
        }

        public async Task AddUserToChat(Guid chatId, Guid userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            var chat = await _chatService.AddNewUserToChatAsync(chatId, user) 
                ?? throw new HubException("Failed to add user to chat.");
            await Clients.Group(chatId.ToString()).SendAsync("UserAdded", chatId, user.Name);
        }

        public async Task RemoveUserFromChat(Guid chatId, Guid userId)
        {
            await _chatService.RemoveUserFromChatAsync(chatId, userId);
            await Clients.Group(chatId.ToString()).SendAsync("UserRemoved", chatId, userId);
        }

        public async Task DeleteChat(Guid chatId)
        {
            bool deleted = await _chatService.DeleteChatAsync(chatId);
            if (!deleted)
            {
                throw new HubException("Failed to delete chat.");
            }

            await Clients.All.SendAsync("ChatDeleted", chatId);
        }
    }
}
