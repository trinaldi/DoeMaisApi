using DoeMais.Domain.Entities;

namespace DoeMais.Repositories.Interfaces;

public interface IDonationRepository
{
    Task<Donation?> CreateDonationAsync(Donation donation);
    Task<List<Donation>> GetDonationListAsync(long userId);
    Task<Donation?> GetDonationByIdAsync(long id, long userId);
    Task<Donation?> UpdateDonationAsync(Donation donation, long userId);
    Task<bool> DeleteDonationAsync(long id, long userId);
}