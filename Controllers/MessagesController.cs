using ChatApp.Models;
using ChatApp.Services.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMessageById(Guid id)
        {
            var message = await _messageService.GetMessageByIdAsync(id);
            if (message == null) return NotFound();
            return Ok(message);
        }

        [HttpGet("chat/{chatId}")]
        public async Task<IActionResult> GetMessagesByChatId(Guid chatId)
        {
            var messages = await _messageService.GetAllChatMessagesAsync(chatId);
            return Ok(messages);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage([FromBody] Message message)
        {
            if (message == null) return BadRequest();
            var createdMessage = await _messageService.CreateNewMessageAsync(message);
            return CreatedAtAction(nameof(GetMessageById), new { id = createdMessage.Id }, createdMessage);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMessage(Guid id, [FromBody] Message message)
        {
            if (message == null || id != message.Id) return BadRequest();
            var updatedMessage = await _messageService.EditMessageAsync(message);
            if (updatedMessage == null) return NotFound();
            return Ok(updatedMessage);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(Guid id)
        {
            var message = await _messageService.GetMessageByIdAsync(id);
            if (message == null) return NotFound();

            await _messageService.DeleteMessageAsync(id);
            return NoContent();
        }
    }
}
