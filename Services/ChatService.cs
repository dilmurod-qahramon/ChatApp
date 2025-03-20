using ChatApp.Models;
using ChatApp.Repositories.interfaces;
using ChatApp.Services.interfaces;

namespace ChatApp.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;

        public ChatService(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        //public async Task<IEnumerable<Chat>> GetAllUserChatsAsync(Guid userId)
        //{
        //    return await _chatRepository.GetAllUserChatsAsync(userId);
        //}

        public async Task<Chat> GetChatByIdAsync(Guid chatId)
        {
            return await _chatRepository.GetChatByIdAsync(chatId);
        }

        public async Task<Chat> CreateNewChatAsync(Chat chat)
        {
            return await _chatRepository.CreateNewChatAsync(chat);
        }

        public async Task<Chat> UpdateChatAsync(Chat chat)
        {
            return await _chatRepository.UpdateChatAsync(chat);
        }

        public async Task ClearChatHistoryAsync(Guid chatId)
        {
            await _chatRepository.ClearChatHistoryAsync(chatId);
        }

        public async Task<Chat> AddNewUserToChatAsync(Guid chatId, User user)
        {
            return await _chatRepository.AddNewUserToChatAsync(chatId, user);
        }

        public async Task RemoveUserFromChatAsync(Guid chatId, Guid userId)
        {
            await _chatRepository.RemoveUserFromChatAsync(chatId, userId);
        }

        public async Task<bool> DeleteChatAsync(Guid chatId)
        {
            return await _chatRepository.DeleteChatAsync(chatId);
        }
    }
}
