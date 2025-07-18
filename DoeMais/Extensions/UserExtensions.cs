using DoeMais.DTO.User;
using DoeMais.Domain.Entities;

namespace DoeMais.Extensions;

public static class UserExtensions
{
    public static UserProfileDto ToDto(this User user)
    {
        return new UserProfileDto
        {
            AvatarUrl = user.AvatarUrl,
            Name = user.Name,
            Phone = user.Phone,
            Address = user.Address,
            Complement = user.Complement ?? "",
            Neighborhood = user.Neighborhood,
            City = user.City,
            State = user.State,
            ZipCode = user.ZipCode
            
        };
    }
    
    public static UpdateUserDto ToUpdateUserDto(this User user)
    {
        return new UpdateUserDto
        {
            AvatarUrl = user.AvatarUrl,
            Name = user.Name,
            Phone = user.Phone,
            Address = user.Address,
            Complement = user.Complement ?? "",
            Neighborhood = user.Neighborhood,
            City = user.City,
            State = user.State,
            ZipCode = user.ZipCode
            
        };
    }
    
    public static void UpdateFromDto(this User user, UpdateUserDto dto)
    {
        user.AvatarUrl = dto.AvatarUrl ?? user.AvatarUrl;
        user.Name = dto.Name ?? user.Name;
        user.Phone = dto.Phone ?? user.Phone;
        user.Address = dto.Address ?? user.Address;
        user.Complement = dto.Complement ?? user.Complement;
        user.Neighborhood = dto.Neighborhood ?? user.Neighborhood;
        user.City = dto.City ?? user.City;
        user.State = dto.State ?? user.State;
        user.ZipCode = dto.ZipCode ?? user.ZipCode;
    }
    
    public static User Clone(this User user)
    {
        return new User
        {
            UserId = user.UserId,
            Name = user.Name,
            Email = user.Email,
            Phone = user.Phone,
            Address = user.Address,
            AvatarUrl = user.AvatarUrl,
            Complement = user.Complement,
            Neighborhood = user.Neighborhood,
            City = user.City,
            State = user.State,
            ZipCode = user.ZipCode
        };
    }
}
