using DoeMais.DTO.Auth;
using DoeMais.Domain.User;
using DoeMais.Services.Interfaces.Utils;

namespace DoeMais.Extensions;

public static class RegisterUserDtoExtensions
{
    public static User ToUser(this RegisterUserDto dto, IPasswordHasher _hasher)
    {
        
        return new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = _hasher.HashPassword(dto.Password),
            Role = dto.Role
        };
    }
}