using DoeMais.Domain.Entities;
using DoeMais.Tests.Domain;

namespace DoeMais.Tests.Extensions;

public static class FakeUserExtensions
{
    public static User ToUser(this FakeUser fake)
    {
        return new User
        {
            UserId = fake.UserId,
            AvatarUrl = fake.AvatarUrl,
            Name = fake.Name,
            Email = fake.Email,
            Phone = fake.Phone,
            Cpf = fake.Cpf,
            Address = fake.Address,
            Complement = fake.Complement,
            Neighborhood = fake.Neighborhood,
            City = fake.City,
            State = fake.State,
            ZipCode = fake.ZipCode,
            PasswordHash = fake.PasswordHash,
            Role = fake.Role 
        };
    }
}