using ChatApp.Models;

namespace ChatApp.Repositories.interfaces
{
    public interface IMessageRepository
    {
        Task<IEnumerable<Message>> GetAllChatMessagesAsync(Guid chatId);
        Task<Message?> GetMessageByIdAsync(Guid id);
        Task<Message> CreateNewMessageAsync(Message message);
        Task<Message> EditMessageAsync(Message message);
        Task DeleteMessageAsync(Guid id);
    }
}
