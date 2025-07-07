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
    public async Task<JournalEntry> SaveOrUpdateJournal(int userId, string journalEntry, DateTime dateTime)
    {
        JournalEntry? entry = await _context.JournalEntries.FirstOrDefaultAsync(j => j.UserId == userId && j.Date.Date == dateTime);

        if (entry != null)
        {
            entry.Content = journalEntry;
            _context.JournalEntries.Update(entry);
        }
        else
        {
            entry = new()
            {
                UserId = userId,
                Date = dateTime,
                Content = journalEntry,

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
        return entry;
    }

    /// <summary>
    /// Adds a summary to a journal entry for a user
    /// </summary>
    /// <param name="userId">The ID of the user to add the summary for</param>
    /// <param name="dateTime">The date and time of the journal entry to add the summary to</param>
    public async Task AddSummaryToJournal(int userId, DateTime dateTime)
    {
        var entry = await _context.JournalEntries.FirstOrDefaultAsync(journal =>
            journal.UserId == userId &&
            journal.Date == dateTime);

        if (entry != null)
        {
            string summary = await _aITherapist.Summarize(entry.Content);

            if (string.IsNullOrWhiteSpace(summary))
            {
                Console.WriteLine("Info: Failed to create summary");
                return;
            }

            entry.Summary = summary;
            await _context.SaveChangesAsync();
        }
        else
        {
            Console.WriteLine("Info: Couldn't find Entry");
        }

        Console.WriteLine("Info: Summary Saved!");
    }

    private async Task AddSummaryToJournal(JournalEntry entry)
    {
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

    public async Task SaveOrUpdateAndSummarizeAsync(int userId, string content, DateTime date)
    {
        var journal = await SaveOrUpdateJournal(userId, content, date);
        await AddSummaryToJournal(journal);
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

        if (entry == null) {
            Console.WriteLine("Conversation does not exist!");
            return;
        }

        ConversationMessage message = new() {
            ConversationId = entry.Id,
            Content = content,
            Role = role,
            SentAt = DateTime.UtcNow
        };

        _context.ConversationMessages.Add(message);
        
        await _context.SaveChangesAsync();
        Console.WriteLine("Info: Message Saved!");
    }
}