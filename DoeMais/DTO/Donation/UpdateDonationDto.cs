using DoeMais.Domain.Enums;

namespace DoeMais.DTO.Donation;

public class UpdateDonationDto
{
    public long AddressId { get; set; }
    public long UserId { get; set; }
    public long DonationId { get; set; }
    
    public string? Title { get; set; } 
    public string? Description { get; set; } 
    public int? Quantity { get; set; } 
    public Status? Status { get; set; }
}