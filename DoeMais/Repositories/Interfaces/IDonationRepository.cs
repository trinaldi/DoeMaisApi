using DoeMais.Domain.Entities;
using DoeMais.Domain.Enums;

namespace DoeMais.Repositories.Interfaces;

public interface IDonationRepository
{
    Task<Donation?> CreateDonationAsync(Donation donation);
    Task<List<Donation>> GetDonationListAsync();
    Task<Donation?> GetDonationByIdAsync(long id);
    Task<Donation?> UpdateDonationAsync(long donationId, Donation donation);
    Task<bool> DeleteDonationAsync(long id);
    Task SaveChangesAsync();
}