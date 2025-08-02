using DoeMais.Data;
using DoeMais.Domain.Entities;
using DoeMais.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DoeMais.Repositories;

public class DonationRepository : IDonationRepository
{
    private readonly AppDbContext _ctx;

    public DonationRepository(AppDbContext ctx)
    {
        _ctx = ctx;
    }
    public async Task<Donation?> CreateDonationAsync(Donation donation)
    {
        await _ctx.Donations.AddAsync(donation);
        await _ctx.SaveChangesAsync();
        
        var donationWithIncludes = await _ctx.Donations
            .Include(d => d.Address)
            .Include(d => d.User)
            .FirstOrDefaultAsync(d => d.DonationId == donation.DonationId);

        return donationWithIncludes;
    }

    public async Task<List<Donation>> GetDonationListAsync(long userId)
    {
        return await _ctx.Donations.Where(d => d.UserId == userId)
            .Include(d => d.Address)
            .Include(d => d.User)
            .ToListAsync();
    }

    public async Task<Donation?> GetDonationByIdAsync(long id, long userId)
    {
        return await _ctx.Donations
            .Where(d => d.DonationId == id && d.UserId == userId)
            .Include(d => d.Address)
            .Include(d => d.User)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> DeleteDonationAsync(long id, long userId)
    {
        var donation = await _ctx.Donations
            .Where(d => d.DonationId == id && d.UserId == userId)
            .FirstOrDefaultAsync();
        if (donation == null) return false;
        
        _ctx.Donations.Remove(donation);
        await _ctx.SaveChangesAsync();
        
        return true;
    }

    public async Task<Donation?> UpdateDonationAsync(Donation donation, long userId)
    {
        _ctx.Donations.Update(donation);
        await _ctx.SaveChangesAsync();
        
        return await _ctx.Donations
            .Include(d => d.Address)
            .Include(d => d.User)
            .FirstOrDefaultAsync(d => d.DonationId == donation.DonationId);
    }
}