using Application.Interfaces.Repositories;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;

namespace Infrastructure.HttpServices;

public class AuthRepository(HttpClient httpClient) : IAuthRepository
{
    private readonly HttpClient _httpClient = httpClient;
    public async Task<IAuthRepository.LoginResponse?> LoginAsync(IAuthRepository.LoginRequest request, CancellationToken cancellationToken)
    {
        using var response = await _httpClient.PostAsJsonAsync("api/auth", request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
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
