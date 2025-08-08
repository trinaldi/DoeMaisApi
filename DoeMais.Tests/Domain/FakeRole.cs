using Bogus;

namespace DoeMais.Tests.Domain;

public class FakeRole
{
    public long RoleId { get; set; }
    public string? Name { get; private set; }

    public ICollection<FakeUserRole> FakeUserRoles { get; set; } = new List<FakeUserRole>();

    public static FakeRole Create(int? id = null, string? name = null)
    {
        var faker = new Faker<FakeRole>("pt_BR")
            .RuleFor(r => r.RoleId, f => id ?? f.Random.Long(1000, 99999))
            .RuleFor(r => r.Name, f => name ?? f.PickRandom("Admin", "Donor", "Charity"));

        return faker.Generate();
    }
}