using Application.Interfaces.Repositories;
using FluentResults;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http.Json;

namespace Infrastructure.HttpServices;

public class AuthRepository(HttpClient httpClient) : IAuthRepository
{
    private readonly HttpClient _httpClient = httpClient;
    public async Task<Result<IAuthRepository.LoginResponse?>> LoginAsync(IAuthRepository.LoginRequest request, CancellationToken cancellationToken)
    {
        using var response = await _httpClient.PostAsJsonAsync("api/auth", request, cancellationToken);
        if (response.StatusCode == HttpStatusCode.Unauthorized)
            return Result.Fail("Usuário ou senha inválidos.");
        if (!response.IsSuccessStatusCode)
            return Result.Fail("Houve algum erro inesperado.");
        var loginResponse = await response.Content.ReadFromJsonAsync<IAuthRepository.LoginResponse>(cancellationToken: cancellationToken);
        return loginResponse;

    }
    public class Options
    {
        public const string SectionName = "AuthRepository";
        [Required]
        public required string BaseUrl { get; set; }
        [Range(1, 30)]
        public required int TimeoutSeconds { get; set; }
    }
}
