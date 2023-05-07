using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using RealWorldExample.Results;
using RealWorldExample.Support;

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
        TryOperationBuilder
            .Handle<SecurityTokenException>().WithError(new ErrorResult("Token", "Security error"))
            .Handle<InvalidOperationException>()
            .WithNoMoreHandlers()
            .WithDefaultError(new ErrorResult("Token", "invalid content"))
            .Execute(() => ValidateToken(token));

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
