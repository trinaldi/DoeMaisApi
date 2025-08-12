using Bogus;
using DoeMais.Domain.Enums;

namespace DoeMais.Tests.Domain;

public class FakeDonation
{
    public long DonationId { get; set; }
    public long UserId { get; set; }
    public long AddressId { get; set; }
    
    public FakeAddress FakeAddress { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public int? Quantity { get; set; }
    public Status Status { get; set; }
    public Category Category { get; set; }
    
    public ICollection<string> Images { get; set; } = new List<string>();

    public static FakeDonation Create(long? userId = null, long? addressId = null)
    {
        var faker = new Faker<FakeDonation>("pt_BR")
            .RuleFor(d => d.DonationId, f => f.IndexGlobal + 1)
            .RuleFor(d => d.UserId, _ => userId ?? 1)
            .RuleFor(d => d.AddressId, f => addressId ?? 1)
            .RuleFor(d => d.Title, f => f.Commerce.ProductName())
            .RuleFor(d => d.Description, f => f.Lorem.Sentence())
            .RuleFor(d => d.Quantity, f => f.Random.Int(1, 10))
            .RuleFor(d => d.Status, f => f.PickRandom<Status>())
            .RuleFor(d => d.Category, f => f.PickRandom<Category>())
            .RuleFor(d => d.Images, f => new List<string>
            {
                f.Image.PicsumUrl(),
                f.Image.PicsumUrl()
            });
        
        return faker.Generate();
    } 
    
    public static List<FakeDonation> CreateMany(long? userId = null, long? addressId = null, int qty = 2)
    {
        var faker = new Faker<FakeDonation>("pt_BR")
            .RuleFor(d => d.DonationId, f => f.IndexGlobal + 1)
            .RuleFor(d => d.UserId, _ => userId ?? 1)
            .RuleFor(d => d.AddressId, f => addressId ?? 1)
            .RuleFor(d => d.Title, f => f.Commerce.ProductName())
            .RuleFor(d => d.Description, f => f.Lorem.Sentence())
            .RuleFor(d => d.Quantity, f => f.Random.Int(1, 10))
            .RuleFor(d => d.Status, f => f.PickRandom<Status>())
            .RuleFor(d => d.Category, f => f.PickRandom<Category>())
            .RuleFor(d => d.Images, f => new List<string>
            {
                f.Image.PicsumUrl(),
                f.Image.PicsumUrl()
            });
        
        return faker.Generate(qty);
    } 
    
    public FakeDonation WithAddress(FakeAddress? address = null)
    {
        var donationId = this.DonationId;
        address ??= FakeAddress.Create();

        this.FakeAddress = address;
        return this;
    }
}