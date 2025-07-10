public class JournalStateService
{
    public int JournalId { get; private set; }

    public event Func<int, Task>? OnJournalChangedAsync;

    public async Task SetJournalIdAsync(int newId)
    {
        if (JournalId != newId)
        {
            JournalId = newId;

            if (OnJournalChangedAsync != null)
            {
                await OnJournalChangedAsync.Invoke(newId);
            }
        }
    }
}
