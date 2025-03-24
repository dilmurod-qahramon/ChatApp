using ChatApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.DataAccess;
public class ChatDbContext(DbContextOptions<ChatDbContext> options) : DbContext(options)
{
    public DbSet<Chat> Chats { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<ChatUsers> ChatUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChatUsers>()
            .HasKey(cu => new { cu.ChatId, cu.UserId });

        base.OnModelCreating(modelBuilder);
    }
}
