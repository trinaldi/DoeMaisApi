namespace DoeMais.Domain.Entities;

public class Address
{
    public long AddressId { get; set; }
    public long UserId { get; set; }
    
    public string Street { get; set; } = "";
    public string? Complement { get; set; }
    public string Neighborhood { get; set; } = "";
    public string City { get; set; } = "";
    public string State { get; set; } = "";
    public string ZipCode { get; set; } = "";
    public bool IsPrimary { get; set; } = true;

    public User User { get; set; } = null!;
    public ICollection<Donation> Donations { get; set; } = [];
}