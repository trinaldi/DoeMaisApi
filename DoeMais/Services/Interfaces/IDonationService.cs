using DoeMais.Common;
using DoeMais.DTO.Donation;

namespace DoeMais.Services.Interfaces;

public interface IDonationService
{
    Task<Result<DonationDto?>> CreateDonationAsync(CreateDonationDto donation, long userId);
    Task<Result<List<DonationDto>>?> GetDonationListAsync(long userId);
    Task<Result<DonationDto?>> GetDonationByIdAsync(long id, long userId);
    Task<Result<DonationDto?>> UpdateDonationAsync(UpdateDonationDto donation, long userId);
    Task<Result<bool>> DeleteDonationAsync(long id, long userId);
}