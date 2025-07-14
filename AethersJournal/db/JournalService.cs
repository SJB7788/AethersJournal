using AethersJournal.Migrations;
using Microsoft.EntityFrameworkCore;

public class JournalService
{
    private JournalContext _context;
    private FreeAITherapist _aITherapist;

    public JournalService(JournalContext context, FreeAITherapist aITherapist)
    {
        _context = context;
        _aITherapist = aITherapist;
    }

    /// <summary>
    /// Gets the journal entry for the current day for a user   
    /// </summary>
    /// <param name="userId">The ID of the user to get the entry for</param>
    /// <returns>The journal entry for the current day for the user, or null if no entry exists</returns>
    public async Task<JournalEntry?> GetTodaysEntryForUser(int userId)
    {
        DateTime today = DateTime.UtcNow.Date;
        return await _context.JournalEntries.FirstOrDefaultAsync(journal =>
            journal.UserId == userId &&
            journal.Date == today);
    }

    /// <summary>
    /// Gets the journal entry for a specific date for a user
    /// </summary>
    /// <param name="userId">The ID of the user to get the entry for</param>
    /// <param name="date">The date to get the entry for</param>
    /// <returns>The journal entry for the specified date for the user, or null if no entry exists</returns>
    public async Task<JournalEntry?> GetEntryForUser(int userId, DateTime? date)
    {
        return await _context.JournalEntries.FirstOrDefaultAsync(journal =>
            journal.UserId == userId &&
            journal.Date == date
        );
    }

    /// <summary>
    /// Saves or updates a journal entry for a user
    /// </summary>
    /// <param name="userId">The ID of the user to save the entry for</param>
    /// <param name="journalEntry">The content of the journal entry</param>
    /// <param name="dateTime">The date and time of the journal entry</param>
    public async Task<(JournalEntry entry, string? originalContent)> SaveOrUpdateJournal(int userId, string journalTitle, string journalEntry, DateTime dateTime)
    {
        JournalEntry? entry = await _context.JournalEntries.FirstOrDefaultAsync(j => j.UserId == userId && j.Date.Date == dateTime);

        string? originalContent = null;

        if (entry != null)
        {
            // save original content
            originalContent = entry.Content;

            entry.Content = journalEntry;
            entry.Title = journalTitle;
            _context.JournalEntries.Update(entry);
        }
        else
        {
            entry = new()
            {
                UserId = userId,
                Date = dateTime,
                Content = journalEntry,
                Title = journalTitle,

                Conversation = new()
                { // conversation is also created
                    UserId = userId,
                    StartedAt = DateTime.UtcNow
                }
            };

            Console.WriteLine(entry.ToString());

            _context.JournalEntries.Add(entry);
        }

        await _context.SaveChangesAsync();
        return (entry, originalContent);
    }

    private bool ShouldSummarize(string newContent, string? oldContent)
    {
        if (string.IsNullOrWhiteSpace(newContent))
            return false;

        if (string.IsNullOrWhiteSpace(oldContent))
            return true;

        return !string.Equals(oldContent, newContent, StringComparison.Ordinal);
    }

    private async Task AddSummaryToJournal(JournalEntry entry, string? oldContent)
    {
        // check if summary is neccesary
        if (!ShouldSummarize(entry.Content, oldContent))
        {
            return;
        }

        string summary = await _aITherapist.Summarize(entry.Content);

        if (string.IsNullOrWhiteSpace(summary))
        {
            Console.WriteLine("Info: Failed to create summary");
            return;
        }

        entry.Summary = summary;
        await _context.SaveChangesAsync();

        Console.WriteLine("Info: Summary Saved!");
    }

    public async Task<JournalEntry> SaveOrUpdateAndSummarizeAsync(int userId, string title, string content, DateTime date)
    {
        var (journal, originalContent) = await SaveOrUpdateJournal(userId, title, content, date);
        await AddSummaryToJournal(journal, originalContent);

        return journal;
    }

    // create conversation and return conversation id
    public async Task<int> CreateConversation(int userId, int journalId)
    {
        Conversation conversation = new()
        {
            UserId = userId,
            JournalId = journalId,
            StartedAt = DateTime.UtcNow
        };

        _context.Conversations.Add(conversation);

        await _context.SaveChangesAsync();

        return conversation.Id;
    }

    // create ConversationMessage
    public async Task AddConversationMessage(int journalId, string content, ChatProfile role)
    {
        var entry = await _context.Conversations.FirstOrDefaultAsync(c => c.JournalId == journalId);

        if (entry == null)
        {
            Console.WriteLine("Conversation does not exist!");
            return;
        }

        ConversationMessage message = new()
        {
            ConversationId = entry.Id,
            Content = content,
            Role = role,
            SentAt = DateTime.UtcNow
        };

        _context.ConversationMessages.Add(message);

        await _context.SaveChangesAsync();
        Console.WriteLine("Info: Message Saved!");
    }

    public async Task<List<ConversationMessage>?> GetAllConversationMessage(int journalId)
    {
        var entry = await _context.Conversations.FirstOrDefaultAsync(c => c.JournalId == journalId);

        if (entry == null)
        {
            Console.WriteLine("Conversation does not exist!");
            return null;
        }

        return await _context.ConversationMessages.Where(m => m.ConversationId == entry.Id).ToListAsync();
    }

    // get journalID based on UserID and Date
    public async Task<JournalEntry?> GetJournalEntryFromUserIdAndDate(int userId, DateTime date)
    {
        JournalEntry? entry = await _context.JournalEntries.FirstOrDefaultAsync(j => j.UserId == userId && j.Date == date);
        return entry;
    }
}