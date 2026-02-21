public class Calendar
{
    private List<CalendarCell> _calenderCells = new();
    private DateTime _calendarStartDate;

    public Calendar()
    {
        _calendarStartDate = DateTime.UtcNow;
    } 

    public Calendar(DateTime startDate)
    {
        _calendarStartDate = startDate;
        GenerateCalendarCells();
    }

    public void GenerateCalendarCells()
    {
        _calenderCells.Clear();

        int firstDayIndex = (int)_calendarStartDate.DayOfWeek;
        // didn't know you can do that but converts day of week to a integer value

        // padding for previous months cells
        for (int i = 0; i < firstDayIndex; i++)
        {
            _calenderCells.Add(item: new CalendarCell { Date = null, IsCurrentMonth = false });
        }

        int daysInMonth = DateTime.DaysInMonth(_calendarStartDate.Year, _calendarStartDate.Month);
        for (int i = 1; i <= daysInMonth; i++)
        {
            _calenderCells.Add(item: new CalendarCell
            {
                Date = new DateTime(_calendarStartDate.Year, _calendarStartDate.Month, i),
                IsCurrentMonth = true
            });
        }

        // padding for next months cells
        while (_calenderCells.Count < 42)
        {
            _calenderCells.Add(item: new CalendarCell { Date = null, IsCurrentMonth = false });
        }        
    }

    public void SetStartDate(DateTime startDate)
    {
        _calendarStartDate = startDate;
    }

    public DateTime GetCalendarStartDate()
    {
        return _calendarStartDate;
    }

    public List<CalendarCell> GetCalendarCells()
    {
        return _calenderCells;
    }
}

public class CalendarCell
{
    public DateTime? Date { get; set; }
    public bool IsCurrentMonth { get; set; }
}