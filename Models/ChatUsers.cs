using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Models;
[Table("chat_users")]
public class ChatUsers
{
    public Guid UserId { get; set; }
    public Guid ChatId { get; set; }
}
