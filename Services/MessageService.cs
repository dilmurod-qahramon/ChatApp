using ChatApp.Models;
using ChatApp.Repositories.interfaces;
using ChatApp.Services.interfaces;

namespace ChatApp.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<IEnumerable<Message>> GetAllChatMessagesAsync(Guid chatId)
        {
            return await _messageRepository.GetAllChatMessagesAsync(chatId);
        }

        public async Task<Message> GetMessageByIdAsync(Guid id)
        {
            return await _messageRepository.GetMessageByIdAsync(id);
        }

        public async Task<Message> CreateNewMessageAsync(Message message)
        {
            return await _messageRepository.CreateNewMessageAsync(message);
        }

        public async Task<Message> EditMessageAsync(Message message)
        {
            return await _messageRepository.EditMessageAsync(message);
        }

        public async Task DeleteMessageAsync(Guid id)
        {
            await _messageRepository.DeleteMessageAsync(id);
        }


    }
}
