using ChatApp.DTOs;
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
        private readonly IUserService _userService;
        public ChatsController(IChatService chatService, IUserService userService)
        {
            _chatService = chatService;
            _userService = userService;
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
        public async Task<IActionResult> CreateChat([FromBody] ChatDto chatDto)
        {
            Chat chat = new()
            {
                Name = chatDto.Name,
                OwnerId = chatDto.OwnerId,
            };
            var createdChat = await _chatService.CreateNewChatAsync(chat);
            return CreatedAtAction(nameof(GetChatById), new { chatId = createdChat.Id }, createdChat);
        }

        [HttpPost("{chatId}/users")]
        public async Task<IActionResult> AddUserToChat(Guid chatId, Guid userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if(user == null) {  return NotFound("User is not found!"); }
            var updatedChat = await _chatService.AddNewUserToChatAsync(chatId, user);
            return Ok(updatedChat);
        }
        
        [HttpPut("{chatId}")]
        public async Task<IActionResult> UpdateChat(Guid chatId, [FromBody] ChatDto chatDto)
        {
            var chat = await _chatService.GetChatByIdAsync(chatId);
            if (chat == null)
                return NotFound();
            if (chatDto.OwnerId != chat.OwnerId)
                return BadRequest("Owner id does not matcht");
            chat.Name = chatDto.Name;
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

        [HttpDelete("{chatId}/users/{userId}")]
        public async Task<IActionResult> RemoveUserFromChat(Guid chatId, Guid userId)
        {
            await _chatService.RemoveUserFromChatAsync(chatId, userId);
            return Ok();
        }

        [HttpDelete("{chatId}/messages")]
        public async Task<IActionResult> ClearChatHistory(Guid chatId)
        {
            await _chatService.ClearChatHistoryAsync(chatId);
            return NoContent();
        }
    }
}
