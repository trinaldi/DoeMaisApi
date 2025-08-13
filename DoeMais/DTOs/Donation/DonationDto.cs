using System.Net.Mime;
using DoeMais.Domain.Entities;
using DoeMais.Domain.Enums;
using DoeMais.DTOs.Address;
using DoeMais.DTOs.User;

namespace DoeMais.DTOs.Donation;

public record DonationDto
{
    
    public long DonationId { get; init; }
    
    public long UserId { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public int? Quantity { get; init; }
    public Status Status { get; init; }
    public Category Category { get; init; }
    public AddressSnapshot AddressSnapshot { get; set; }
    public ICollection<string>? Images { get; init; } = new List<string>();
}