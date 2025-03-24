using ChatApp.Models;
using ChatApp.Repositories.interfaces;
using ChatApp.Services.interfaces;

namespace ChatApp.Services;

public class MessageService(IMessageRepository messageRepository) : IMessageService
{
    public async Task<IEnumerable<Message>> GetAllChatMessagesAsync(Guid chatId)
    {
        return await messageRepository.GetAllChatMessagesAsync(chatId);
    }

    public async Task<Message> GetMessageByIdAsync(Guid id)
    {
        return await messageRepository.GetMessageByIdAsync(id);
    }

    public async Task<Message> CreateNewMessageAsync(Message message)
    {
        return await messageRepository.CreateNewMessageAsync(message);
    }

    public async Task<Message> EditMessageAsync(Message message)
    {
        return await messageRepository.EditMessageAsync(message);
    }

    public async Task DeleteMessageAsync(Guid id)
    {
        await messageRepository.DeleteMessageAsync(id);
    }


}
