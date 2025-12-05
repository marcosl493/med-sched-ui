using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebUi.Extensions;

public class JwtClaimsParser
{
    public static ClaimsPrincipal ToClaimsPrincipal(string jwt)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwt);
        var claims = token.Claims.ToList();
        claims.Add(new Claim("access_token", jwt));
        var identity = new ClaimsIdentity(
            claims,
            authenticationType: "jwt"
        );

        return new ClaimsPrincipal(identity);
    }
}
