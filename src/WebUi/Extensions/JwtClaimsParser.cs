using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebUi.Extensions;

public class JwtClaimsParser
{
    public static ClaimsPrincipal ToClaimsPrincipal(string jwt)
    {
         var handler = new JwtSecurityTokenHandler();
         var token = handler.ReadJwtToken(jwt);

        var identity = new ClaimsIdentity(
            token.Claims,
            authenticationType: "jwt"
        );

        return new ClaimsPrincipal(identity);
    }
}
