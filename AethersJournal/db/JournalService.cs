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

    public async Task<JournalEntry?> GetTodaysEntryForUser(int userId)
    {
        DateTime today = DateTime.UtcNow.Date;
        return await _context.JournalEntries.FirstOrDefaultAsync(journal =>
            journal.UserId == userId &&
            journal.Date == today);
    }

    public async Task<JournalEntry?> GetEntryForUser(int userId, DateTime? date)
    {
        return await _context.JournalEntries.FirstOrDefaultAsync(journal =>
            journal.UserId == userId &&
            journal.Date == date
        );
    }

    public async Task SaveOrUpdateJournal(int userId, string journalEntry, DateTime dateTime)
    {
        JournalEntry? entry = await _context.JournalEntries.FirstOrDefaultAsync(j => j.UserId == userId && j.Date.Date == dateTime);

        if (entry != null)
        {
            entry.Content = journalEntry;
            _context.JournalEntries.Update(entry);
        }
        else
        {
            JournalEntry newEntry = new()
            {
                UserId = userId,
                Date = dateTime,
                Content = journalEntry
            };

            _context.JournalEntries.Add(newEntry);
        }

        await _context.SaveChangesAsync();
    }

    public async Task AddSummaryToJournal(int userId, DateTime dateTime)
    {
        var entry = await _context.JournalEntries.FirstOrDefaultAsync(journal =>
            journal.UserId == userId &&
            journal.Date == dateTime);

        if (entry != null)
        {
            string summary = await _aITherapist.Summarize(entry.Content);

            if (summary == "")
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
}