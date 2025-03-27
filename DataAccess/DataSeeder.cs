using ChatApp.Models;

namespace ChatApp.DataAccess;

public class DataSeeder(ChatDbContext context)
{
    public void SeedData()
    {
        if(!context.Chats.Any())
            context.Chats.AddRange(GetTestChatData());
        if(!context.Messages.Any())
            context.Messages.AddRange(GetTestMessageData());
        if(!context.ChatUsers.Any())
            context.ChatUsers.AddRange(GetTestChatUsersData());

        context.SaveChanges();
    }

    private static List<Chat> GetTestChatData()
    {
        return [
            new() { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "TestChat1", OwnerId = Guid.Parse("22222222-2222-2222-2222-111111111111") },
            new() { Id = Guid.Parse("11111111-1111-1111-1111-222222222222"), Name = "TestChat2", OwnerId = Guid.Parse("22222222-2222-2222-2222-222222222222") },
            new() { Id = Guid.Parse("11111111-1111-1111-1111-333333333333"), Name = "TestChat3", OwnerId = Guid.Parse("22222222-2222-2222-2222-333333333333") },
            new() { Id = Guid.Parse("11111111-1111-1111-1111-444444444444"), Name = "TestChat4", OwnerId = Guid.Parse("22222222-2222-2222-2222-333333333333") },
        ];
    }

    private static List<Message> GetTestMessageData()
    {
        return [
            new() { Id = Guid.Parse("33333333-3333-3333-3333-111111111111"), Text = "TestMessage1", UserId = Guid.Parse("22222222-2222-2222-2222-111111111111"), ChatId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            new() { Id = Guid.Parse("33333333-3333-3333-3333-222222222222"), Text = "TestMessage2", UserId = Guid.Parse("22222222-2222-2222-2222-222222222222"), ChatId = Guid.Parse("11111111-1111-1111-1111-222222222222") },
            new() { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Text = "TestMessage3", UserId = Guid.Parse("22222222-2222-2222-2222-333333333333"), ChatId = Guid.Parse("11111111-1111-1111-1111-333333333333") },
            new() { Id = Guid.Parse("33333333-3333-3333-3333-444444444444"), Text = "TestMessage4", UserId = Guid.Parse("22222222-2222-2222-2222-333333333333"), ChatId = Guid.Parse("11111111-1111-1111-1111-444444444444") },
        ];
    }

    private static List<ChatUsers> GetTestChatUsersData()
    {
        return [
            new() { UserId = Guid.Parse("22222222-2222-2222-2222-111111111111"), ChatId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            new() { UserId = Guid.Parse("22222222-2222-2222-2222-222222222222"), ChatId = Guid.Parse("11111111-1111-1111-1111-222222222222") },
            new() { UserId = Guid.Parse("22222222-2222-2222-2222-333333333333"), ChatId = Guid.Parse("11111111-1111-1111-1111-333333333333") },
            new() { UserId = Guid.Parse("22222222-2222-2222-2222-333333333333"), ChatId = Guid.Parse("11111111-1111-1111-1111-444444444444") },
        ];
    }

}
