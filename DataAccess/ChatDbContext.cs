using ChatApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.DataAccess;
public class ChatDbContext(DbContextOptions<ChatDbContext> options) : DbContext(options)
{
    public DbSet<Chat> Chats { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<User> Users { get; set; }
}
