public class JournalEntry
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Content { get; set; } = string.Empty;

    // Foreign Key
    public int UserId { get; set; }
    public User User { get; set; } = default!;
}
