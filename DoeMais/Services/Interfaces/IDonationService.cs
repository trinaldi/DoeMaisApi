using DoeMais.Common;
using DoeMais.DTO.Donation;

namespace DoeMais.Services.Interfaces;

public interface IDonationService
{
    Task<Result<DonationDto?>> CreateDonationAsync(CreateDonationDto donation);
    Task<Result<List<DonationDto>>?> GetDonationListAsync();
    Task<Result<DonationDto?>> GetDonationByIdAsync(long id);
    Task<Result<DonationDto?>> UpdateDonationAsync(UpdateDonationDto donation);
    Task<Result<bool>> DeleteDonationAsync(long id);
}