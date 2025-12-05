using Domain.ValueObjects;
using FluentResults;

namespace Application.Interfaces.Repositories;

public interface IMedSchedRepository
{
    Task<Result> CreateAvailableSlotAsync(IEnumerable<DateRange> ranges, Guid physicianId, CancellationToken cancellationToken);
}
