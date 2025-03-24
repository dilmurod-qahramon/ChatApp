using ChatApp.DataAccess;
using ChatApp.Models;
using ChatApp.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Repositories;
public class ChatRepository(ChatDbContext context) : IChatRepository
{
    public async Task<Chat> GetChatByIdAsync(Guid chatId)
        {
            return await context.Chats
                .Include(chat => chat.Users) 
                .Include(chat => chat.Messages) 
                .FirstOrDefaultAsync(chat => chat.Id == chatId);
        }
   
    public async Task<Chat> CreateNewChatAsync(Chat chat)
    {
        await context.Chats.AddAsync(chat);
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
    }

    public async Task<Chat> AddNewUserToChatAsync(Guid chatId, User user)
    {
        var chat = await context.Chats.Include(c => c.Users).FirstOrDefaultAsync(c => c.Id == chatId);
        chat.Users.Add(user);
        await context.SaveChangesAsync();
        return chat;
    }

    public async Task RemoveUserFromChatAsync(Guid chatId, Guid userId)
    {
        var chat = await context.Chats.Include(c => c.Users).FirstOrDefaultAsync(c => c.Id == chatId);
        var user = chat.Users.FirstOrDefault(u => u.Id == userId);
        chat.Users.Remove(user);
        await context.SaveChangesAsync();
    }
    
    public async Task<bool> DeleteChatAsync(Guid chatId)
    {
        var chat = await context.Chats.FindAsync(chatId);
        if (chat == null) return false;

        context.Chats.Remove(chat);
        await context.Chats.Where(chat => chat.Id == chatId).ExecuteDeleteAsync();
        await context.SaveChangesAsync();
        return true;
    }
}
