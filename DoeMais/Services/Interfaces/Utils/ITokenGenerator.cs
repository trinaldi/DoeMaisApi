using DoeMais.Models;

namespace DoeMais.Services.Interfaces.Utils;

public interface ITokenGenerator
{
    string GenerateToken(User user);
}
