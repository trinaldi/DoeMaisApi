using DoeMais.Common;
using DoeMais.DTOs.Donation;

namespace DoeMais.Services.Interfaces;

public interface IDonationService
{
    Task<Result<DonationDto?>> CreateDonationAsync(CreateDonationDto donation);
    Task<Result<List<DonationDto>>?> GetDonationListAsync();
    Task<Result<DonationDto?>> GetDonationByIdAsync(long id);
    Task<Result<DonationDto?>> UpdateDonationAsync(long donationId, UpdateDonationDto donation);
    Task<Result<bool>> DeleteDonationAsync(long id);
    Task<Result<bool>> ChangeStatusAsync(long donationId, DonationStatusDto statusDto);
    Task<Result<List<DonationDto>>> GetDonationsByCategoryAsync(int category);
}