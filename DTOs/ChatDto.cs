using System.ComponentModel.DataAnnotations;

namespace ChatApp.DTOs
{
    public class ChatDto
    {
        [MaxLength(200)]
        public string? Name { get; set; }
        [Required]
        public Guid OwnerId { get; set; }

    }
}
