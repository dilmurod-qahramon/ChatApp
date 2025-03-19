using System.ComponentModel.DataAnnotations;

namespace ChatApp.DTOs
{
    public class ChatDto
    {
        [Key]
        public Guid Id { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }
    }
}
