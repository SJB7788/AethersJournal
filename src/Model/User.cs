using Microsoft.AspNetCore.Identity;

public class User : IdentityUser
{
    public List<JournalEntry> JournalEntries { get; set; } = new();
    public List<Conversation> Conversations { get; set; } = new();
}
