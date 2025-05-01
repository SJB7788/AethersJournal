public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public List<JournalEntry> JournalEntries { get; set; } = new();
}
