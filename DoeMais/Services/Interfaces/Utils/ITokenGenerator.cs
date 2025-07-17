using DoeMais.Domain.Entities;

namespace DoeMais.Services.Interfaces.Utils;

public interface ITokenGenerator
{
    string GenerateToken(User user);
}
