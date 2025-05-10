using Microsoft.EntityFrameworkCore;

public class JournalService
{
    public readonly JournalContext _context;

    public JournalService(JournalContext context)
    {
        _context = context;
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
}