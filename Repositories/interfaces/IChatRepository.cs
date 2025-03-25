using ChatApp.Models;

namespace ChatApp.Repositories.interfaces;

public interface IChatRepository
{
    Task<IList<Guid>> GetUserChatIdsAsync(Guid userId);
    Task<Chat?> GetChatByIdAsync(Guid chatId);
    Task<bool> IsMemberOfChat(Guid userId, Guid chatId);
    Task<Chat> CreateNewChatAsync(Chat chat);
    Task<Chat> UpdateChatAsync(Chat chat);
    Task ClearChatHistoryAsync(Guid chatId);
    Task<ChatUsers> AddNewUserToChatAsync(Guid chatId, Guid userId);
    Task RemoveUserFromChatAsync(Guid chatId, Guid userId);
    Task DeleteChatAsync(Guid chatId);
}
