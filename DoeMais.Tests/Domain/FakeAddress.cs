using Bogus;
using Bogus.Extensions.Brazil;

namespace DoeMais.Tests.Domain;
public class FakeAddress
{
    public long AddressId { get; set; }
    public string Street { get; set; } = "";
    public string? Complement { get; set; }
    public string Neighborhood { get; set; } = "";
    public string City { get; set; } = "";
    public string State { get; set; } = "";
    public string ZipCode { get; set; } = "";
    public bool IsPrimary { get; set; } = true;
    public ICollection<FakeDonation> FakeDonations { get; set; } = new List<FakeDonation>();
    
    public static FakeAddress Create(int? addressId = null, int qty = 1)
    {
        var faker = new Faker<FakeAddress>("pt_BR")
            .RuleFor(u => u.AddressId, f => addressId ?? f.Random.Long(1000, 99999))
            .RuleFor(u => u.Street, f => f.Address.StreetAddress())
            .RuleFor(u => u.Complement, f => f.Random.Bool(0.5f) ? f.Address.SecondaryAddress() : null)
            .RuleFor(u => u.Neighborhood, f => f.Address.County())
            .RuleFor(u => u.City, f => f.Address.City())
            .RuleFor(u => u.State, f => f.Address.StateAbbr())
            .RuleFor(u => u.IsPrimary, f => f.Random.Bool())
            .RuleFor(u => u.ZipCode, f => f.Address.ZipCode());

        return faker.Generate();
    }
    
    public static List<FakeAddress> CreateMany(int? addressId = null, int qty = 1)
    {
        var faker = new Faker<FakeAddress>("pt_BR")
            .RuleFor(u => u.AddressId, f => addressId ?? f.Random.Long(1000, 99999))
            .RuleFor(u => u.AddressId, f => addressId ?? f.Random.Long(1000, 99999))
            .RuleFor(u => u.Street, f => f.Address.StreetAddress())
            .RuleFor(u => u.Complement, f => f.Random.Bool(0.5f) ? f.Address.SecondaryAddress() : null)
            .RuleFor(u => u.Neighborhood, f => f.Address.County())
            .RuleFor(u => u.City, f => f.Address.City())
            .RuleFor(u => u.State, f => f.Address.StateAbbr())
            .RuleFor(u => u.IsPrimary, f => f.Random.Bool())
            .RuleFor(u => u.ZipCode, f => f.Address.ZipCode());

        return faker.Generate(qty);
    }
    
    public FakeAddress WithDonation(FakeDonation? donation = null)
    {
        var addressId = this.AddressId;
        donation ??= FakeDonation.Create(null, addressId);

        this.FakeDonations.Add(donation);
        return this;
    }
}
