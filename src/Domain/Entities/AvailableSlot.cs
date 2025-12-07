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


            for (int i = 1; i <= WeeksToRepeat; i++)
            {
                var baseWeek = Range.StartTime.AddDays(i * 7);
                var startWeek = baseWeek.AddDays(-(int)baseWeek.DayOfWeek);
                foreach (var day in Days)
                {
                    yield return new AvailableSlot(
                        new DateRange(
                            startWeek.AddDays((int)day).AddHours(Range.StartTime.Hour).AddMinutes(Range.StartTime.Minute),
                            startWeek.AddDays((int)day).AddHours(Range.EndTime.Hour).AddMinutes(Range.EndTime.Minute)
                        ),
                        false,
                        0,
                        Days
                    );
                }
            }
        }

        var avaliableFirstSlot = new AvailableSlot(
                        Range,
                        false,
                        0,
                        Days
                    );
        var list = new List<AvailableSlot> { avaliableFirstSlot };
        foreach (var item in list)
        {
            yield return item;
        }
        
    }
}
