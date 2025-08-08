using System.Collections;
using Bogus;
using Bogus.Extensions.Brazil;
using DoeMais.Domain.Entities;
using DoeMais.Domain.ValueObjects;

namespace DoeMais.Tests.Domain;

public class FakeUser
{
    public long UserId { get; set; }
    public string? AvatarUrl { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = "";
    public Cpf Cpf { get; set; }
    public string PasswordHash { get; set; } = default!;
    
    public ICollection<FakeUserRole> FakeUserRoles { get; set; } = new List<FakeUserRole>();
    public ICollection<FakeDonation> FakeDonations { get; set; } = new List<FakeDonation>();
    public ICollection<FakeAddress> FakeAddresses { get; set; } = new List<FakeAddress>();

    public static FakeUser Create(long? userId = null)
    {
        var idBase = userId ?? 1;

        var faker = new Faker<FakeUser>("pt_BR")
            .RuleFor(u => u.UserId, f => idBase)
            .RuleFor(u => u.AvatarUrl, f => f.Internet.Avatar())
            .RuleFor(u => u.Name, f => f.Name.FullName())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Phone, f => f.Phone.PhoneNumber())
            .RuleFor(u => u.Cpf, f => new Cpf(ExtensionsForBrazil.Cpf(f.Person, false)))
            .RuleFor(u => u.PasswordHash, f => f.Internet.Password(60));

        var fakeUser = faker.Generate();
        
        var fakeRoles = new List<FakeUserRole>
        {
            FakeUserRole.Create(fakeUser)
        };
        
        fakeUser.FakeUserRoles = fakeRoles;

        var fakeAddresses = new List<FakeAddress>
        {
            FakeAddress.Create()
        };
        fakeUser.FakeAddresses = fakeAddresses;
        
        var fakeDonations = new List<FakeDonation>
        {
            FakeDonation.Create()
        };
        fakeUser.FakeDonations = fakeDonations;

        return fakeUser;
    }
    
    public FakeUser WithAddress(FakeAddress? address = null)
    {
        address ??= FakeAddress.Create();
        FakeAddresses.Add(address);
        return this;
    }

    public FakeUser WithDonation(FakeDonation? donation = null)
    {
        if (FakeAddresses.Count == 0)
            WithAddress();

        var addressId = FakeAddresses.First().AddressId;
        donation ??= FakeDonation.Create(UserId, addressId);

        FakeDonations.Add(donation);
        return this;
    }
}