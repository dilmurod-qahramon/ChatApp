using ChatApp.Models;
using ChatApp.Services.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatsController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserChats(Guid userId)
        {
            var chats = await _chatService.GetChatByIdAsync(userId);
            return Ok(chats);
        }

        [HttpGet("{chatId}")]
        public async Task<IActionResult> GetChatById(Guid chatId)
        {
            var chat = await _chatService.GetChatByIdAsync(chatId);
            if (chat == null) return NotFound();
            return Ok(chat);
        }

        [HttpPost]
        public async Task<IActionResult> CreateChat([FromBody] Chat chat)
        {
            var createdChat = await _chatService.CreateNewChatAsync(chat);
            return CreatedAtAction(nameof(GetChatById), new { chatId = createdChat.Id }, createdChat);
        }

        [HttpPut("{chatId}")]
        public async Task<IActionResult> UpdateChat(Guid chatId, [FromBody] Chat chat)
        {
            var updatedChat = await _chatService.UpdateChatAsync(chat);
            if (updatedChat == null) return NotFound();
            return Ok(updatedChat);
        }

        [HttpDelete("{chatId}")]
        public async Task<IActionResult> DeleteChat(Guid chatId)
        {
            var success = await _chatService.DeleteChatAsync(chatId);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpPost("{chatId}/users")]
        public async Task<IActionResult> AddUserToChat(Guid chatId, [FromBody] User user)
        {
            var updatedChat = await _chatService.AddNewUserAsync(chatId, user);
            if (updatedChat == null) return NotFound();
            return Ok(updatedChat);
        }

        [HttpDelete("{chatId}/users/{userId}")]
        public async Task<IActionResult> RemoveUserFromChat(Guid chatId, Guid userId)
        {
            var updatedChat = await _chatService.RemoveUserAsync(chatId, userId);
            if (updatedChat == null) return NotFound();
            return Ok(updatedChat);
        }

        [HttpDelete("{chatId}/messages")]
        public async Task<IActionResult> ClearChatHistory(Guid chatId)
        {
            await _chatService.ClearChatHistoryAsync(chatId);
            return NoContent();
        }
    }
}
