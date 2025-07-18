using DoeMais.DTO.Auth;
using DoeMais.Domain.Entities;
using DoeMais.Services.Interfaces.Utils;

namespace DoeMais.Extensions;

public static class RegisterUserDtoExtensions
{
    public static User ToUser(this RegisterUserDto dto, IPasswordHasher _hasher)
    {
        
        var user =  new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = _hasher.HashPassword(dto.Password),
        };
        
        user.UserRoles.Add(new UserRole
        {
            RoleId = 2,
            User = user,
        });
        
        return user;
    }
}