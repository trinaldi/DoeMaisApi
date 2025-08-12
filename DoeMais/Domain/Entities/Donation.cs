using DoeMais.Domain.Enums;
using System.Collections.Generic;
using DoeMais.Domain.Interfaces;

namespace DoeMais.Domain.Entities;

public class Donation : IUserOwned
{
    public long DonationId { get; set; }
    public long UserId { get; set; }
    public long AddressId { get; set; }
    
    public User User { get; set; }
    public Address Address { get; set; }
    public Category Category { get; set; }
    
    public string Title { get; set; }
    public string? Description { get; set; }
    public int? Quantity { get; set; }
    public Status Status { get; set; }
    public ICollection<string>? Images { get; set; } = [];
}