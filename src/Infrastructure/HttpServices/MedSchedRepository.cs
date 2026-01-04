using Application.Interfaces.Repositories;
using Domain.ValueObjects;
using FluentResults;
using System;
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

    public async Task<Result> DeleteScheduleAsync(Guid physicianId, Guid scheduleId, CancellationToken cancellationToken)
    {

        using var response = await _httpClient.DeleteAsync($"api/physicians/{physicianId}/schedules/{scheduleId}", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return Result.Fail("Houve algum erro ao deletar os horários. Verifique seus horários, e tente novamente.");
        }

        return Result.Ok();
    }

    public async Task<Result<IMedSchedRepository.GetAllAppointmentDto?>> GetAllAppointmentsAsync(int top, Guid? physicianId, IMedSchedRepository.AppointmentStatus? status, Guid? patientId, int? skip, CancellationToken cancellationToken)
    {
        var queryParts = new List<string>();
        if (physicianId.HasValue)
            queryParts.Add($"physicianId={Uri.EscapeDataString(physicianId.Value.ToString())}");
        if (patientId.HasValue)
            queryParts.Add($"patientId={Uri.EscapeDataString(patientId.Value.ToString())}");
        if (skip.HasValue)
            queryParts.Add($"skip={skip.Value}");
        if (status.HasValue)
            queryParts.Add($"status={(int)status}");

        queryParts.Add($"top={top}");
        var url = "api/appointments" + (queryParts.Count > 0 ? "?" + string.Join("&", queryParts) : string.Empty);
        using var response = await _httpClient.GetAsync(url, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return Result.Fail("Erro ao obter os horários. Tente novamente mais tarde.");
        }
        var result = await response.Content.ReadFromJsonAsync<IMedSchedRepository.GetAllAppointmentDto>(cancellationToken);

        return Result.Ok(result);
    }

    public async Task<Result<IMedSchedRepository.PatientDto?>> GetPatientByIdAsync(Guid Id, CancellationToken cancellationToken)
    {
        var resource = $"api/patients/{Id}";
        using var response = await _httpClient.GetAsync(resource, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return Result.Fail("Erro ao obter os dados do paciente. Tente novamente mais tarde.");
        }
        var result = await response.Content.ReadFromJsonAsync<IMedSchedRepository.PatientDto>(cancellationToken);
        return result;
    }

    public async Task<Result<IMedSchedRepository.GetAllSchedulesDto?>> GetSchedulesAsync(Guid? physicianId, int? skip, DateTime? startTime, int? top, CancellationToken cancellationToken)
    {
        var queryParts = new List<string>();
        if (physicianId.HasValue)
            queryParts.Add($"physicianId={Uri.EscapeDataString(physicianId.Value.ToString())}");
        if (skip.HasValue)
            queryParts.Add($"skip={skip.Value}");
        if (startTime.HasValue)
            queryParts.Add($"startTime={Uri.EscapeDataString(startTime.Value.ToString("o"))}");
        if (top.HasValue)
            queryParts.Add($"top={top.Value}");

        var url = "api/schedules" + (queryParts.Count > 0 ? "?" + string.Join("&", queryParts) : string.Empty);

        using var response = await _httpClient.GetAsync(url, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return Result.Fail("Erro ao obter os horários. Tente novamente mais tarde.");
        }
        var result = await response.Content.ReadFromJsonAsync<IMedSchedRepository.GetAllSchedulesDto>(cancellationToken);
        return Result.Ok(result);
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
