public class JournalStateService
{
    public int JournalId { get; private set; }
    public string JournalSummary { get; private set; } = string.Empty;

    public event Func<int, string, Task>? OnJournalChangedAsync;

    public async Task SetJournalInfoAsync(int newId, string entrySummary)
    {
        if (JournalId != newId)
        {
            JournalId = newId;
            JournalSummary = entrySummary;
            Console.WriteLine("This happened");
            if (OnJournalChangedAsync != null)
            {
                await OnJournalChangedAsync.Invoke(newId, JournalSummary);
            }
        }
    }
}
