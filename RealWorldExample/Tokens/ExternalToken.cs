using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using RealWorldExample.Results;
using RealWorldExample.Support;
using System.IdentityModel.Tokens.Jwt;

namespace RealWorldExample.Tokens;

public record ExternalToken
{
    private ExternalToken(string token) => Token = token;

    public string Token { get; }

    public static Result<ExternalToken> Create(string token) =>
    ValidateFormat(token)
        .Bind(() => ValidateContent(token))
        .Map(token => new ExternalToken(token));

    private static Result ValidateFormat(string token) =>
        new JsonWebTokenHandler().CanReadToken(token) ?
            Result.Ok() :
            new ErrorResult("Token", "invalid Format");

    private static Result<string> ValidateContent(string token) =>
        TryOperation.New
            .Handle<InvalidCastException>()
            .For(token)
            .WithError(new ErrorResult("Token", "invalid token"))
            .Try(ValidateToken);

    private static string ValidateToken(string token)
    {
        TokenValidationParameters tokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidIssuer = "archsoft.com",
            ValidateAudience = true,
            ValidAudience = "serviceincloud",
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
        _ = new JwtSecurityTokenHandler().ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

        return securityToken.ToString()!;
    }
}
