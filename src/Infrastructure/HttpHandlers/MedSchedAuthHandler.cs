using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace Infrastructure.HttpHandlers;

public class MedSchedAuthHandler(IHttpContextAccessor accessor) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = accessor.HttpContext?.User?
            .FindFirst("access_token")?.Value;


        if (!string.IsNullOrEmpty(token))
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

        return await base.SendAsync(request, cancellationToken);
    }
}

