using DoeMais.Domain.Entities;
using DoeMais.Domain.ValueObjects;
using DoeMais.Tests.Domain;

namespace DoeMais.Tests.Extensions;

public static class FakeUserExtensions
{
    public static User ToUser(this FakeUser fake)
    {
        var user = new User
        {
            UserId = fake.UserId,
            AvatarUrl = fake.AvatarUrl,
            Name = fake.Name,
            Email = fake.Email,
            Phone = fake.Phone,
            Cpf = fake.Cpf,
            PasswordHash = fake.PasswordHash,
            Addresses = fake.FakeAddresses
                .Select(a => new Address
                {
                    Street = a.Street,
                    City = a.City,
                    State = a.State,
                    Neighborhood = a.Neighborhood,
                    ZipCode = a.ZipCode,
                    IsPrimary = a.IsPrimary
                }).ToList()
        };

        user.UserRoles = fake.FakeUserRoles
            .Select(fur => new UserRole
            {
                RoleId = fur.RoleId,
                Role = new Role(fur.RoleId, fur?.Role?.Name),
                User = user
            }).ToList();
        
        return user;
    }
}