using FluentResults;

namespace Application.Interfaces.Repositories;

public interface IAuthRepository
{
    public sealed record LoginResponse(string AccessToken, int ExpiresIn, string TokenType);
    public sealed record LoginRequest(string email, string password);
    Task<Result<LoginResponse?>> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
}
