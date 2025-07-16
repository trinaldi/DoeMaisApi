using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DoeMais.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Int64 GetUserId(this ClaimsPrincipal user)
    {
        var sub = user?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (sub == null)
            throw new UnauthorizedAccessException("Token inválido: não encontrado.");

        return Int64.Parse(sub);
    }
}