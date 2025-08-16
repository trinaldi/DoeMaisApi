using DoeMais.Domain.Enums;
using DoeMais.DTOs.Donation;

namespace DoeMais.Extensions;

public static class DonationStatusDtoExtensions
{
    
    public static Status ToStatus(this DonationStatusDto statusDto)
    {
        return statusDto.Status;
    }
}