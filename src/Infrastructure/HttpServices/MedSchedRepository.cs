using Application.Interfaces.Repositories;

namespace Infrastructure.HttpServices;

public class MedSchedRepository(HttpClient httpClient) : IMedSchedRepository
{
    private readonly HttpClient _httpClient = httpClient;

}
