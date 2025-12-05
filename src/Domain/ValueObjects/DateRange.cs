namespace Domain.ValueObjects;

public class DateRange
{
    public DateTime StartTime { get; }
    public DateTime EndTime { get; }
    public DateRange(DateTime start, DateTime end)
    {
        if (end < start)
            throw new ArgumentException("End date must be greater than or equal to start date.", nameof(end));
        StartTime = start;
        EndTime = end;
    }
}
