using System.Net.Mime;
using DoeMais.Domain.Enums;
using DoeMais.DTO.Address;
using DoeMais.DTO.User;

namespace DoeMais.DTO.Donation;

public class DonationDto
{
    
    public long DonationId { get; set; }
    
    public UserProfileDto? User { get; set; }
    public AddressDto? Address { get; set; }
    
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int? Quantity { get; set; }
    public Status Status { get; set; }
    public ICollection<string>? Images { get; set; } = new List<string>();
}