using DoeMais.Domain.Enums;

namespace DoeMais.DTO.Donation;

public record ListDonationDto
{
    
    public long DonationId { get; init; }
    public string? Title { get; init; }
    public Status Status { get; init; }
    public string? City { get; init; }
    public string? State { get; init; }
    public int ImageCount { get; init; }
    public DateTime CreatedAt { get; init; }
}