using Application.Interfaces.Repositories;
using Domain.ValueObjects;
using FluentResults;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;

namespace Infrastructure.HttpServices;

public class MedSchedRepository(HttpClient httpClient) : IMedSchedRepository
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<Result> CreateAvailableSlotAsync(IEnumerable<DateRange> ranges, Guid physicianId, CancellationToken cancellationToken)
    {
        var success = true;
        await Parallel.ForEachAsync(ranges, cancellationToken, async (slot, ct) =>
        {
            using var response = await _httpClient.PostAsJsonAsync($"api/physicians/{physicianId}/schedules", slot, ct);
            if (!response.IsSuccessStatusCode)
            {
                success = false;
            }
        });
        if (!success)
        {
            return Result.Fail("Houve algum erro ao criar os horários disponíveis. Verifique seus horários, e tente novamente.");
        }
        return Result.Ok();
    }
    public class Options
    {
        public const string SectionName = "MedSchedRepository";
        [Required]
        public required string BaseUrl { get; set; }
        [Range(1, 30)]
        public required int TimeoutSeconds { get; set; }
    }
}
