using Domain.ValueObjects;
using FluentResults;
namespace Application.Interfaces.Repositories;

public interface IMedSchedRepository
{
    Task<Result> CreateAvailableSlotAsync(IEnumerable<DateRange> ranges, Guid physicianId, CancellationToken cancellationToken);
    Task<Result<IEnumerable<ScheduleDto>>> GetSchedulesAsync(Guid? physicianId, int? skip, DateTime? startTime, int? top, CancellationToken cancellationToken);


    public record ScheduleDto(
        Guid Id,
        PhysicianDto Physician,
        bool IsAvaliable,
        DateTimeOffset StartTime,
        DateTimeOffset EndTime
    );
    public record PhysicianDto(
        Guid Id,
        string Name,
        string Specialty
        );
}
