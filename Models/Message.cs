using System.ComponentModel.DataAnnotations;

namespace ChatApp.Models
{
    public class Message
    {
        [Key]
        public Guid Id { get; set; }
        [MaxLength(10_000)]
        public string? Text { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid ChatId { get; set; }
        [Required]
        public Chat Chat { get; set; }
    }
}
