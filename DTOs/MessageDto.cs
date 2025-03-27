using System.ComponentModel.DataAnnotations;

namespace ChatApp.DTOs;
public class MessageDto
{
    [Key]
    public Guid Id { get; set; }
    [MaxLength(10_000)]
    [MinLength(1)]
    public string? Text { get; set; }
    public string? ImageUrl { get; set; }

    [Required]
    public Guid UserId { get; set; }
    [Required]
    public Guid ChatId { get; set; }
}
