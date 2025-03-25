using ChatApp.Models;
using ChatApp.Repositories.interfaces;
using ChatApp.Services.interfaces;

namespace ChatApp.Services;

public class ChatService(IChatRepository chatRepository) : IChatService
{
    public async Task<Chat?> GetChatByIdAsync(Guid chatId)
    {
        return await chatRepository.GetChatByIdAsync(chatId);
    }

    public async Task<bool> IsMemberOfChat(Guid userId, Guid chatId)
    {
        return await chatRepository.IsMemberOfChat(userId, chatId);
    }

    public async Task<Chat> CreateNewChatAsync(Chat chat)
    {
        return await chatRepository.CreateNewChatAsync(chat);
    }

    public async Task<Chat> UpdateChatAsync(Chat chat)
    {
        return await chatRepository.UpdateChatAsync(chat);
    }

    public async Task ClearChatHistoryAsync(Guid chatId)
    {
        await chatRepository.ClearChatHistoryAsync(chatId);
    }

    public async Task<ChatUsers> AddNewUserToChatAsync(Guid chatId, Guid userId)
    {
        return await chatRepository.AddNewUserToChatAsync(chatId, userId);
    }

    public async Task RemoveUserFromChatAsync(Guid chatId, Guid userId)
    {
        await chatRepository.RemoveUserFromChatAsync(chatId, userId);
    }

    public async Task DeleteChatAsync(Guid chatId)
    {
        await chatRepository.DeleteChatAsync(chatId);
    }

    public async Task<IList<Guid>> GetUserChatIdsAsync(Guid userId)
    {
        return await chatRepository.GetUserChatIdsAsync(userId);
    }
}
