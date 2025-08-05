using DoeMais.Domain.Entities;

namespace DoeMais.Repositories.Interfaces;

public interface IDonationRepository
{
    Task<Donation?> CreateDonationAsync(Donation donation);
    Task<List<Donation>> GetDonationListAsync();
    Task<Donation?> GetDonationByIdAsync(long id);
    Task<Donation?> UpdateDonationAsync(Donation donation);
    Task<bool> DeleteDonationAsync(long id);
}