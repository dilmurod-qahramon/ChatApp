using ChatApp.DataAccess;
using ChatApp.Models;
using ChatApp.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Repositories;

    public class ChatRepository : IChatRepository
    {
        private readonly ChatDbContext _context;

        public ChatRepository(ChatDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Chat>> GetAllUserChatsAsync(Guid userId)
        {
            return await _context.Chats
                 .Where(chat => chat.Users.Any(user => user.Id == userId)) 
                 .Include(chat => chat.Messages)
                 .ToListAsync();
        }

        public async Task<Chat> GetChatByIdAsync(Guid chatId)
        {
            return await _context.Chats
                .Include(chat => chat.Users) 
                .Include(chat => chat.Messages) 
                .FirstOrDefaultAsync(chat => chat.Id == chatId);
        }
        public async Task<Chat> CreateNewChatAsync(Chat chat)
        {
            await _context.Chats.AddAsync(chat);
            await _context.SaveChangesAsync();
            return chat;
        }
        public async Task<Chat> UpdateChatAsync(Chat chat)
        {
            _context.Chats.Update(chat);
            await _context.SaveChangesAsync();
            return chat;
        }

        public async Task ClearChatHistoryAsync(Guid chatId)
        {
            var chat = await _context.Chats.FindAsync(chatId);
            if (chat == null) return;

            chat.Messages.Clear();
            await _context.SaveChangesAsync();
        }

        public async Task<Chat> AddNewUserAsync(Guid chatId, User user)
        {
            var chat = await _context.Chats.Include(c => c.Users).FirstOrDefaultAsync(c => c.Id == chatId);
            if (chat == null) return null;

            chat.Users.Add(user);
            await _context.SaveChangesAsync();
            return chat;
        }

        public async Task<Chat> RemoveUserAsync(Guid chatId, Guid userId)
        {
            var chat = await _context.Chats.Include(c => c.Users).FirstOrDefaultAsync(c => c.Id == chatId);
            if (chat == null) return null;

            var user = chat.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null) return null;

            chat.Users.Remove(user);
            await _context.SaveChangesAsync();
            return chat;
        }

        public async Task<bool> DeleteChatAsync(Guid chatId)
        {
            var chat = await _context.Chats.FindAsync(chatId);
            if (chat == null) return false;

            _context.Chats.Remove(chat);
            await _context.SaveChangesAsync();
            return true;
        }
}
