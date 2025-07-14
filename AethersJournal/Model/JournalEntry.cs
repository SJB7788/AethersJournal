public class JournalEntry
{
    public int Id { get; set; }
    public int UserId { get; set; } // Foreign Key
    public int ConversationId { get; set; } // Foreign Key
    public DateTime Date { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Summary {get; set;} = string.Empty;
    public User User { get; set; } = default!;
    public Conversation Conversation { get; set; } = default!;

}
