using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class JournalContext : IdentityDbContext<User>
{
    public JournalContext(DbContextOptions<JournalContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<JournalEntry>()
            .HasOne(j => j.User)
            .WithMany(u => u.JournalEntries)
            .HasForeignKey(j => j.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<JournalEntry>()
            .HasOne(j => j.Conversation)
            .WithOne()
            .HasForeignKey<JournalEntry>(j => j.ConversationId)
            .OnDelete(DeleteBehavior.SetNull);

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
    public DbSet<JournalEntry> JournalEntries => Set<JournalEntry>();
    public DbSet<Conversation> Conversations => Set<Conversation>();
    public DbSet<ConversationMessage> ConversationMessages => Set<ConversationMessage>();
}
