using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DoeMais.Services.Query;

namespace DoeMais.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public long UserId
    {
        get
        {
            var claimValue = _httpContextAccessor.HttpContext?.User?.Claims
                .FirstOrDefault(c => c.Type is JwtRegisteredClaimNames.Sub or ClaimTypes.NameIdentifier)
                ?.Value;

            return long.TryParse(claimValue, out var id) ? id : 0;
        }
    }
}