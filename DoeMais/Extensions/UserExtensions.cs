using DoeMais.DTOs.User;
using DoeMais.Domain.Entities;
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
            Addresses = user.Addresses
                .Select(a => new AddressDto
                {
                    AddressId = a.AddressId,
                    Street = a.Street,
                    Complement = a.Complement,
                    Neighborhood = a.Neighborhood,
                    City = a.City,
                    State = a.State,
                    ZipCode = a.ZipCode,
                    IsPrimary = a.IsPrimary
                })
                .ToList()
        };
    }
    
    public static UpdateUserDto ToUpdateUserDto(this User user)
    {
        return new UpdateUserDto
        {
            AvatarUrl = user.AvatarUrl,
            Name = user.Name,
            Phone = user.Phone,
            Addresses = user.Addresses
                .Select(a => new AddressDto
                {
                    AddressId = a.AddressId,
                    Street = a.Street,
                    Complement = a.Complement,
                    Neighborhood = a.Neighborhood,
                    City = a.City,
                    State = a.State,
                    ZipCode = a.ZipCode,
                    IsPrimary = a.IsPrimary
                })
                .ToList()
            
        };
    }
    
    public static void UpdateFromDto(this User user, UpdateUserDto dto)
    {
        user.AvatarUrl = dto.AvatarUrl ?? user.AvatarUrl;
        user.Name = dto.Name ?? user.Name;
        user.Phone = dto.Phone ?? user.Phone;
        
        var addressDto = dto.Addresses.FirstOrDefault(a => a.IsPrimary);
        if (addressDto == null) return;
        
        var address = user.Addresses.FirstOrDefault(a => a.IsPrimary);

        if (address != null)
        {
            address.Street = addressDto.Street;
            address.Complement = addressDto.Complement;
            address.Neighborhood = addressDto.Neighborhood;
            address.City = addressDto.City;
            address.State = addressDto.State;
            address.ZipCode = addressDto.ZipCode;
        }
        else
        {
            user.Addresses.Add(new Address
            {
                Street = addressDto.Street,
                Complement = addressDto.Complement,
                Neighborhood = addressDto.Neighborhood,
                City = addressDto.City,
                State = addressDto.State,
                ZipCode = addressDto.ZipCode,
                IsPrimary = true,
                UserId = user.UserId
            });
        }
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
            Addresses = user.Addresses
                .Select(a => new Address
                {
                    AddressId = a.AddressId,
                    Street = a.Street,
                    Complement = a.Complement,
                    Neighborhood = a.Neighborhood,
                    City = a.City,
                    State = a.State,
                    ZipCode = a.ZipCode,
                    IsPrimary = a.IsPrimary,
                    UserId = user.UserId
                })
                .ToList()
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
