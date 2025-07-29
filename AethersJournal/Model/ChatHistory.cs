public class Conversation {
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty; // Foreign Key
    public int JournalId {get; set; } // Foreign Key
    public DateTime StartedAt { get; set; }
    public User User { get; set; } = default!;
    public List<ConversationMessage> Messages { get; set; } = new();
}

public class ConversationMessage {
    public int Id { get; set; }
    public int ConversationId { get; set; } // Foreign Key
    public string Content { get; set; } = string.Empty;
    public ChatProfile Role { get; set; }
    public DateTime SentAt  { get; set; }
    public Conversation Conversation { get; set; } = default!;
}
