using DoeMais.DTOs.User;
using DoeMais.Domain.Entities;
using DoeMais.Domain.OwnedTypes;
using DoeMais.DTOs.Address;

namespace DoeMais.Extensions;

public static class UserExtensions
{
    public static UserProfileDto ToDto(this User user)
    {
        return new UserProfileDto
        {
            UserId = user.UserId,
            AvatarUrl = user.AvatarUrl,
            Name = user.Name,
            Phone = user.Phone,
            AddressDto = user.Address.ToDto()
        };
    }
    
    public static UpdateUserDto ToUpdateUserDto(this User user)
    {
        return new UpdateUserDto
        {
            AvatarUrl = user.AvatarUrl,
            Name = user.Name,
            Phone = user.Phone,
            AddressDto = user.Address.ToDto()
            
        };
    }
    
    public static void UpdateFromDto(this User user, UpdateUserDto dto)
    {
        user.AvatarUrl = dto.AvatarUrl ?? user.AvatarUrl;
        user.Name = dto.Name ?? user.Name;
        user.Phone = dto.Phone ?? user.Phone;
        user.Address = dto.AddressDto?.ToEntity() ?? user.Address;

    }
    
    public static User Clone(this User user)
    {
        var clonedUser =  new User
        {
            UserId = user.UserId,
            Name = user.Name,
            Email = user.Email,
            Phone = user.Phone,
            Cpf = user.Cpf,
            Address = user.Address
        };
        
        foreach (var userRole in user.UserRoles)
        {
            clonedUser.UserRoles.Add(new UserRole
            {
                RoleId = userRole.RoleId,
                UserId = clonedUser.UserId
            });
        }
        
        return clonedUser;
    }
}
