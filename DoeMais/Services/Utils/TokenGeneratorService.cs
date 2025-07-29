using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DoeMais.Domain.Entities;
using DoeMais.Services.Interfaces.Utils;
using Microsoft.IdentityModel.Tokens;

namespace DoeMais.Services.Utils;

public class TokenGeneratorService : ITokenGenerator
{
    private readonly IConfiguration _config;

    public TokenGeneratorService(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        
        claims.AddRange(user.UserRoles.Select(ur => ur.Role.Name).Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddHours(3),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}