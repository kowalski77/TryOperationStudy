using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RealWorldExample.Tokens;

public static class JwtTokenFactory
{
    public static string Create()
    {
        Claim[] claims = new[]
        {
            new Claim("app-id", Guid.NewGuid().ToString()),
        };

        JwtSecurityToken token = new(
            "archsoft.com",
            "serviceincloud",
            claims,
            expires: DateTime.Now.AddMinutes(10));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
