using ChatApp.DataAccess;
using ChatApp.Models;
using ChatApp.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Repositories;
public class ChatRepository(ChatDbContext context) : IChatRepository
{
    public async Task<Chat?> GetChatByIdAsync(Guid chatId)
        {
            return await context.Chats
                .Include(chat => chat.Messages) 
                .FirstOrDefaultAsync(chat => chat.Id == chatId);
        }

    public async Task<bool> IsMemberOfChat(Guid userId, Guid chatId)
    {
        return await context.ChatUsers.AnyAsync(cu => cu.UserId == userId && cu.ChatId == chatId);
    }
   
    public async Task<Chat> CreateNewChatAsync(Chat chat)
    {
        await context.Chats.AddAsync(chat);
        await context.ChatUsers.AddAsync(new() { ChatId = chat.Id, UserId = chat.OwnerId });
        await context.SaveChangesAsync();
        return chat;
    }
    
    public async Task<Chat> UpdateChatAsync(Chat chat)
    {
        context.Chats.Update(chat);
        await context.SaveChangesAsync();
        return chat;
    }

    public async Task ClearChatHistoryAsync(Guid chatId)
    {
        await context.Messages
            .Where(m => m.ChatId == chatId)
            .ExecuteDeleteAsync();
        await context.SaveChangesAsync();
    }

    public async Task<ChatUsers> AddNewUserToChatAsync(Guid chatId, Guid userId)
    {
        var chatuser = await context.ChatUsers.AddAsync(new() { ChatId = chatId, UserId = userId });
        await context.SaveChangesAsync();
        return chatuser.Entity;
    }

    public async Task RemoveUserFromChatAsync(Guid chatId, Guid userId)
    {
        var chatUser = await context.ChatUsers.FirstOrDefaultAsync(x => x.ChatId == chatId && x.UserId == userId);
        if (chatUser != null)
        {
            context.ChatUsers.Remove(chatUser);
            await context.SaveChangesAsync();
        }
    }
    
    public async Task DeleteChatAsync(Guid chatId)
    {
        await context.ChatUsers.Where(cu => cu.ChatId == chatId).ExecuteDeleteAsync();
        await context.Chats.Where(chat => chat.Id == chatId).ExecuteDeleteAsync(); 
        await context.SaveChangesAsync();
    }
}
