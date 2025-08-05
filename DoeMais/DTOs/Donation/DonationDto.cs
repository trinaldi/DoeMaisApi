using System.Net.Mime;
using DoeMais.Domain.Enums;
using DoeMais.DTO.Address;
using DoeMais.DTO.User;

namespace DoeMais.DTO.Donation;

public record DonationDto
{
    
    public long DonationId { get; init; }
    
    public long UserId { get; init; }
    public long AddressId { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public int? Quantity { get; init; }
    public Status Status { get; init; }
    public ICollection<string>? Images { get; init; } = new List<string>();
}