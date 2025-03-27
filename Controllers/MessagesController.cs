using ChatApp.DTOs;
using ChatApp.Models;
using ChatApp.Services.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MessagesController(IMessageService messageService, IChatService chatService, IUserService userService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult<MessageDto>> GetMessageById(Guid id)
    {
        var message = await messageService.GetMessageByIdAsync(id);
        if (message == null) return NotFound();

        var dto = new MessageDto()
        {
            Id = message.Id,
            ChatId = message.ChatId,
            UserId = message.UserId,
            Text = message.Text
        };
        return Ok(dto);
    }

    [HttpGet("chat/{chatId}")]
    public async Task<IActionResult> GetAllMessagesInAChat(Guid chatId)
    {
        var messages = await messageService.GetAllChatMessagesAsync(chatId);
        return Ok(messages);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMessage([FromBody] MessageDto messageDto)
    {
        var message = new Message()
        {
            Text = messageDto.Text?.Trim(),
            UserId = messageDto.UserId,
            ChatId = messageDto.ChatId,
            ImageUrl = messageDto.ImageUrl?.Trim(),
        };
        var createdMessage = await messageService.CreateNewMessageAsync(message);
        return CreatedAtAction(nameof(GetMessageById), new { id = createdMessage.Id }, createdMessage);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMessage(Guid id, [FromBody] MessageDto messageDto)
    {
        if (messageDto == null || id != messageDto.Id) return BadRequest();

        var chat = chatService.GetChatByIdAsync(messageDto.ChatId);
        if(chat == null) return NotFound("chat is not  found!");

        var message = await messageService.GetMessageByIdAsync(id);
        message.Text = messageDto.Text?.Trim();
        var updatedMessage = await messageService.EditMessageAsync(message);
        return Ok(updatedMessage);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMessage(Guid id, Guid userId)
    {
        var message = await messageService.GetMessageByIdAsync(id);
        if (message == null) return NotFound();
        if(message.Id == userId) return BadRequest("User is not the owner of the message");
        await messageService.DeleteMessageAsync(id);
        return NoContent();
    }
}
