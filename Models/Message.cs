﻿using System.ComponentModel.DataAnnotations;

namespace ChatApp.Models;

public class Message
{
    [Key]
    public Guid Id { get; set; }
    [MaxLength(10_000)]
    public string? Text { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime SentAt { get; set; } = DateTime.UtcNow;

    [Required]
    public Guid UserId { get; set; }
    public User User { get; set; }
    [Required]
    public Guid ChatId { get; set; }
    public Chat Chat { get; set; }
}
