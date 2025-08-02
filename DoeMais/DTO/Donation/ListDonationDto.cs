using DoeMais.Domain.Enums;

namespace DoeMais.DTO.Donation;

public class ListDonationDto
{
    
    public long DonationId { get; set; }
    public string? Title { get; set; }
    public Status Status { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public int ImageCount { get; set; }
    public DateTime CreatedAt { get; set; }
}