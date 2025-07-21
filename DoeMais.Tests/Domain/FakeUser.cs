using Bogus;
using Bogus.Extensions.Brazil;
using DoeMais.Domain.Entities;

namespace DoeMais.Tests.Domain;

public class FakeUser
{
    public Int64 UserId { get; set; }
    public string? AvatarUrl { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = "";
    public string Cpf { get; set; } = "";
    public string Address { get; set; } = "";
    public string? Complement { get; set; }
    public string Neighborhood { get; set; } = "";
    public string City { get; set; } = "";
    public string State { get; set; } = "";
    public string ZipCode { get; set; } = "";
    public string PasswordHash { get; set; } = default!;
    
    public ICollection<FakeUserRole> FakeUserRoles { get; set; } = new List<FakeUserRole>();

    public static FakeUser Create(long? userId = null)
    {
        var idBase = userId ?? 1;

        var faker = new Faker<FakeUser>("pt_BR")
            .RuleFor(u => u.UserId, f => idBase)
            .RuleFor(u => u.AvatarUrl, f => f.Internet.Avatar())
            .RuleFor(u => u.Name, f => f.Name.FullName())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Phone, f => f.Phone.PhoneNumber())
            .RuleFor(u => u.Cpf, f => f.Person.Cpf(false))
            .RuleFor(u => u.Address, f => f.Address.StreetAddress())
            .RuleFor(u => u.Complement, f => f.Random.Bool(0.5f) ? f.Address.SecondaryAddress() : null)
            .RuleFor(u => u.Neighborhood, f => f.Address.County())
            .RuleFor(u => u.City, f => f.Address.City())
            .RuleFor(u => u.State, f => f.Address.StateAbbr())
            .RuleFor(u => u.ZipCode, f => f.Address.ZipCode())
            .RuleFor(u => u.PasswordHash, f => f.Internet.Password(60));

        var fakeUser = faker.Generate();
        
        var fakeRoles = new List<FakeUserRole>
        {
            FakeUserRole.Create(fakeUser)
        };
        
        fakeUser.FakeUserRoles = fakeRoles;

        return fakeUser;
    }
}