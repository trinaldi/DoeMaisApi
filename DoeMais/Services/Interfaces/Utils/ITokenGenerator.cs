using DoeMais.Domain.User;

namespace DoeMais.Services.Interfaces.Utils;

public interface ITokenGenerator
{
    string GenerateToken(User user);
}
