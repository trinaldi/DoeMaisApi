using DoeMais.DTO.Donation;

namespace DoeMais.Services.Interfaces;

public interface IDonationService
{
    Task<DonationDto?> CreateDonationAsync(CreateDonationDto donation, long userId);
    Task<List<DonationDto>> GetDonationListAsync(long userId);
    Task<DonationDto?> GetDonationByIdAsync(long id, long userId);
    Task<DonationDto?> UpdateDonationAsync(UpdateDonationDto donation, long userId);
    Task<bool> DeleteDonationAsync(long id, long userId);
}