using Domain.ValueObjects;
using FluentResults;
namespace Application.Interfaces.Repositories;

public interface IMedSchedRepository
{
    Task<Result> CreateAvailableSlotAsync(IEnumerable<DateRange> ranges, Guid physicianId, CancellationToken cancellationToken);
    Task<Result<GetAllSchedulesDto?>> GetSchedulesAsync(Guid? physicianId, int? skip, DateTime? startTime, int? top, CancellationToken cancellationToken);
    Task<Result<GetAllAppointmentDto?>> GetAllAppointmentsAsync(int top, Guid? physicianId, AppointmentStatus? status, Guid? patientId, int? skip, CancellationToken cancellationToken);
    Task<Result<PatientDto?>> GetPatientByIdAsync(Guid Id, CancellationToken cancellationToken);
    Task<Result> DeleteScheduleAsync(Guid physicianId, Guid scheduleId, CancellationToken cancellationToken);
    public record ScheduleDto(
        Guid Id,
        PhysicianDto Physician,
        bool IsAvaliable,
        DateTimeOffset StartTime,
        DateTimeOffset EndTime
    );
    public record GetAllSchedulesDto(
    IEnumerable<ScheduleDto> Schedules,
    int Count
);

    public record PhysicianDto(
        Guid Id,
        string Name,
        string Specialty
        );
    public enum AppointmentStatus
    {
        SCHEDULED = 1,
        CANCELED
    };
    public record AppointmentDto
    (
        Guid Id,
        PatientDto Patient,
        Guid PhysicianId,
        AppointmentStatus Status,
        DateTimeOffset StartTime,
        DateTimeOffset EndTime,
        DateTimeOffset CreatedAt);
    public record GetAllAppointmentDto
    (
        IEnumerable<AppointmentDto> Appointments,
        int Count
    );
    public record PatientDto(Guid Id, string Name, string Email, DateTimeOffset DateOfBirth);
}
