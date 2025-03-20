using ChatApp.Models;

namespace ChatApp.Repositories.interfaces
{
    public interface IChatRepository
    {
        //Task<IEnumerable<Chat>> GetAllUserChatsAsync(Guid userId);
        Task<Chat> GetChatByIdAsync(Guid chatId);
        Task<Chat> CreateNewChatAsync(Chat chat);
        Task<Chat> UpdateChatAsync(Chat chat);
        Task ClearChatHistoryAsync(Guid chatId);
        Task<Chat> AddNewUserToChatAsync(Guid chatId, User user);
        Task RemoveUserFromChatAsync(Guid chatId, Guid userId);
        Task<bool> DeleteChatAsync(Guid chatId);
    }
}
