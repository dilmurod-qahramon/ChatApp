using ChatApp.DataAccess;
using ChatApp.Models;
using ChatApp.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Repositories
{
    public class MessageRepository(ChatDbContext context) : IMessageRepository
    {
        private readonly ChatDbContext _context = context;

        public async Task<IEnumerable<Message>> GetAllChatMessagesAsync(Guid chatId)
        {
            return await _context.Messages
                .Where(m => m.ChatId == chatId) 
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        public async Task<Message> GetMessageByIdAsync(Guid id)
        {
            return await _context.Messages.FindAsync(id);
        }

        public async Task<Message> CreateNewMessageAsync(Message message)
        {
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<Message> EditMessageAsync(Message message)
        {
            _context.Messages.Update(message);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task DeleteMessageAsync(Guid id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message is not null)
                _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
        }
    }
}
