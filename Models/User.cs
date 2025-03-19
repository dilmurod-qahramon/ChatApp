namespace ChatApp.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Chat> Chats { get; set; } = [];
    }
}
