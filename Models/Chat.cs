using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Models;
[Table("chats")]
public class Chat
{
    [Key]
    public Guid Id { get; set; }
    [MaxLength(200)]
    public string? Name { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid OwnerId { get; set; }
    public ICollection<Message> Messages { get; set; } = [];
}
