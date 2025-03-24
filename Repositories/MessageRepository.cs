using ChatApp.DataAccess;
using ChatApp.Models;
using ChatApp.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Repositories;

public class MessageRepository(ChatDbContext context) : IMessageRepository
{
    public async Task<IEnumerable<Message>> GetAllChatMessagesAsync(Guid chatId)
    {
        return await context.Messages
            .Where(m => m.ChatId == chatId) 
            .OrderBy(m => m.SentAt)
            .ToListAsync();
    }

    public async Task<Message?> GetMessageByIdAsync(Guid id)
    {
        return await context.Messages.FindAsync(id);
    }

    public async Task<Message> CreateNewMessageAsync(Message message)
    {
        await context.Messages.AddAsync(message);
        await context.SaveChangesAsync();
        return message;
    }

    public async Task<Message> EditMessageAsync(Message message)
    {
        context.Messages.Update(message);
        await context.SaveChangesAsync();
        return message;
    }

    public async Task DeleteMessageAsync(Guid id)
    {
        var message = await context.Messages.FindAsync(id);
        if (message is not null)
            context.Messages.Remove(message);
        await context.SaveChangesAsync();
    }
}
