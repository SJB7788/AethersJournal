using Microsoft.EntityFrameworkCore;

public class JournalContext : DbContext
{
    public JournalContext(DbContextOptions<JournalContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<JournalEntry>()
            .HasOne(j => j.User)
            .WithMany(u => u.JournalEntries)
            .HasForeignKey(j => j.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<JournalEntry>()
            .HasOne(j => j.Conversation)
            .WithOne()
            .HasForeignKey<JournalEntry>(j => j.ConversationId);

        modelBuilder.Entity<Conversation>()
            .HasOne(c => c.User)
            .WithMany(u => u.Conversations)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ConversationMessage>()
            .HasOne(m => m.Conversation)
            .WithMany(c => c.Messages)
            .HasForeignKey(m => m.ConversationId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ConversationMessage>()
            .Property(m => m.Role)
            .HasConversion<string>();
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<JournalEntry> JournalEntries => Set<JournalEntry>();
    public DbSet<Conversation> Conversations => Set<Conversation>();
    public DbSet<ConversationMessage> ConversationMessages => Set<ConversationMessage>();
}
