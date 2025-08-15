using DoeMais.Domain.Entities;
using DoeMais.Domain.OwnedTypes;
using DoeMais.Domain.ValueObjects;
using DoeMais.Tests.Domain;

namespace DoeMais.Tests.Extensions;

public static class FakeUserExtensions
{
    public static User ToEntity(this FakeUser fake)
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
            Address = fake.FakeAddress.ToEntity()
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