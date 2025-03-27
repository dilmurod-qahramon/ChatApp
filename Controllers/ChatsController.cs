using ChatApp.DTOs;
using ChatApp.Models;
using ChatApp.Services.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Controllers;

[Route("api/[controller]")]
//[Authorize]
[ApiController]
public class ChatsController(IChatService chatService, IUserService userService) : ControllerBase
{
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<ChatDto>>> GetUserChats(Guid userId)
    {
        var chats = await chatService.GetAllUserChatsByUserIdAsync(userId);
        return Ok(chats.Select(chat => new ChatDto()
        {
            Id = chat.Id,
            Name = chat.Name,
            CreatedAt = chat.CreatedAt,
            OwnerId = chat.OwnerId,
            Messages = chat.Messages
            .Select(message => new MessageDto()
            { Id = message.Id, Text = message.Text, UserId = message.UserId }).ToList(),
        }));
    }

    [HttpGet("{chatId}")]
    public async Task<ActionResult<ChatDto>> GetChatById(Guid chatId)
    {
        var chat = await chatService.GetChatByIdAsync(chatId);
        if (chat == null) return NotFound();
        var chatDto = new ChatDto()
        {
            Id = chat.Id,
            Name = chat.Name,
            OwnerId = chat.OwnerId,
            CreatedAt = chat.CreatedAt,
            Messages = chat.Messages.Select(message => new MessageDto()
            { Id = message.Id, Text = message.Text, UserId = message.UserId }).ToList(),
        };
        return Ok(chatDto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateNewChat([FromBody] ChatDto chatDto)
    {
        Chat chat = new()
        {
            Name = chatDto.Name,
            OwnerId = chatDto.OwnerId,
        };
        var createdChat = await chatService.CreateNewChatAsync(chat);
        return CreatedAtAction(nameof(GetChatById), new { chatId = createdChat.Id }, createdChat);
    }

    [HttpPost("{chatId}")]
    public async Task<IActionResult> AddUserToChat(Guid chatId, Guid userId)
    {
        //only chat owner should be able to add user to chat
        var user = await userService.GetUserByIdAsync(userId);
        if(user == null) return NotFound("User is not found!");

        var chat = await chatService.GetChatByIdAsync(chatId);
        if (chat == null) return NotFound("Chat is not found!");

        var updatedChat = await chatService.AddNewUserToChatAsync(chatId, userId);
        return Ok(updatedChat);
    }
    
    [HttpPut("{chatId}")]
    public async Task<IActionResult> UpdateChat(Guid chatId, [FromBody] ChatDto chatDto)
    {
        var chat = await chatService.GetChatByIdAsync(chatId);
        if (chat == null)
            return NotFound();
        if (chatDto.OwnerId != chat.OwnerId)
            return BadRequest("Owner id does not matcht");
        chat.Name = chatDto.Name;
        var updatedChat = await chatService.UpdateChatAsync(chat);
        return Ok(updatedChat);
    }

    [HttpDelete("{chatId}")]
    public async Task<IActionResult> DeleteChat(Guid chatId)
    {
        var userIdStr = (HttpContext.User?.FindFirst("sub")?.Value)
            ?? throw new UnauthorizedAccessException("User not found.");
        var userId = Guid.Parse(userIdStr);

        var chat = await chatService.GetChatByIdAsync(chatId);
        if (chat == null) return NotFound("Chat is not found!");
        if (chat.OwnerId != userId) BadRequest("Only owner of the chat can delete it");
        await chatService.DeleteChatAsync(chatId);
        return NoContent();
    }

    [HttpDelete("{chatId}/users/{userId}")]
    public async Task<IActionResult> RemoveUserFromChat(Guid chatId)
    {
        var userIdStr = (HttpContext.User?.FindFirst("sub")?.Value)
           ?? throw new UnauthorizedAccessException("User not found.");
        var userId = Guid.Parse(userIdStr);
        var chat = await chatService.GetChatByIdAsync(chatId);
        if (chat == null) return NotFound("Chat is not found!");
        if (chat.OwnerId != userId) BadRequest("Only owner of the chat can remove users");
        if (!await chatService.IsMemberOfChat(userId, chatId))
        {
            BadRequest("User is not a memeber of this chat!");
        }
        await chatService.RemoveUserFromChatAsync(chatId, userId);
        return Ok();
    }

    [HttpDelete("{chatId}/messages")]
    public async Task<IActionResult> ClearChatHistory(Guid chatId)
    {
        var userIdStr = (HttpContext.User?.FindFirst("sub")?.Value)
           ?? throw new UnauthorizedAccessException("User not found.");
        var userId = Guid.Parse(userIdStr);
        var chat = await chatService.GetChatByIdAsync(chatId);
        if (chat == null) return NotFound("Chat is not found!");
        if (chat.OwnerId != userId) BadRequest("Only owner of the chat can clear history");
        await chatService.ClearChatHistoryAsync(chatId);
        return NoContent();
    }
}