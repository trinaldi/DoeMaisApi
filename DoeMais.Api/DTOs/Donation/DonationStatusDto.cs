using DoeMais.Domain.Enums;

namespace DoeMais.DTOs.Donation;

public record DonationStatusDto
{
    public Status Status { get; init; }
}