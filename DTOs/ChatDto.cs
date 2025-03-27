using System.ComponentModel.DataAnnotations;

namespace ChatApp.DTOs;

public class ChatDto
{
    public Guid Id { get; set; }
    [MaxLength(200)]
    public string? Name { get; set; }
    [Required]
    public Guid OwnerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<MessageDto> Messages { get; set; }
}
