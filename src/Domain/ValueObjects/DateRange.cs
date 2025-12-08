namespace Domain.ValueObjects;

public class DateRange
{
    public DateTimeOffset StartTime { get; }
    public DateTimeOffset EndTime { get; }
    public DateRange(DateTimeOffset start, DateTimeOffset end)
    {
        if (end < start)
            throw new ArgumentException("End date must be greater than or equal to start date.", nameof(end));
        StartTime = start;
        EndTime = end;
    }
}
