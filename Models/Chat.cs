using System.ComponentModel.DataAnnotations;


namespace ChatApp.Models
{
    public class Chat
    {
        [Key]
        public Guid Id { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Message> Messages { get; set; } = [];
        public ICollection<User> Users { get; set; } = [];
    }
}
