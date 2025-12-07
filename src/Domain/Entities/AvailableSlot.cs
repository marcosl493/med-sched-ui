using Domain.ValueObjects;

namespace Domain.Entities;

public class AvailableSlot
{
    public DateRange Range { get; }
    public bool RepeatWeekly { get; }
    public int WeeksToRepeat { get; }
    public List<DayOfWeek> Days { get; }
    public AvailableSlot(DateRange range, bool repeatWeekly, int weeksToRepeat, List<DayOfWeek> days)
    {
        if (repeatWeekly && weeksToRepeat <= 0)
            throw new ArgumentException("O número de semanas deve ser maior que zero.");

        Range = range;
        RepeatWeekly = repeatWeekly;
        WeeksToRepeat = weeksToRepeat;
        Days = days;
    }
    public IEnumerable<AvailableSlot> GetAvailableSlots()
    {
        if (RepeatWeekly)
        {
            for (int i = 0; i < WeeksToRepeat; i++)
            {
                var start = Range.StartTime.AddDays(i * 7);
                var end = Range.EndTime.AddDays(i * 7);
                yield return new AvailableSlot(new DateRange(start, end), false, 0, Days);
            }
            yield break;
        }

        // Not repeating: return only the original slot
        yield return new AvailableSlot(Range, false, 0, Days);
    }
}
