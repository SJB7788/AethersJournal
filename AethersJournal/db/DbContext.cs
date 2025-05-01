using Microsoft.EntityFrameworkCore;

public class JournalContext : DbContext
{
    public JournalContext(DbContextOptions<JournalContext> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<JournalEntry> JournalEntries => Set<JournalEntry>();
}
