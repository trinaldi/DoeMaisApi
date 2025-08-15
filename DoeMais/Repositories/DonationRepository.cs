using DoeMais.Domain.Entities;
using DoeMais.Domain.Enums;
using DoeMais.Extensions;
using DoeMais.Infrastructure;
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
            .FirstOrDefaultAsync(d => d.DonationId == donation.DonationId);

        return donationWithIncludes;
    }

    public async Task<List<Donation>> GetDonationListAsync()
    {
        return await _ctx.Donations
            .ToListAsync();
    }

    public async Task<Donation?> GetDonationByIdAsync(long donationId)
    {
        return await _ctx.Donations
            .Where(d => d.DonationId == donationId)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> DeleteDonationAsync(long id)
    {
        var donation = await _ctx.Donations
            .Where(d => d.DonationId == id)
            .FirstOrDefaultAsync();
        if (donation == null) return false;
        
        _ctx.Donations.Remove(donation);
        await _ctx.SaveChangesAsync();
        
        return true;
    }


    public async Task<Donation?> UpdateDonationAsync(long donationId, Donation donation)
    {
        var existingDonation = await _ctx.Donations
            .FirstOrDefaultAsync(d => d.DonationId == donationId);
        if (existingDonation is null) return null;
        
        existingDonation.UpdateFrom(donation);
        await _ctx.SaveChangesAsync();
        
        return await _ctx.Donations
            .FirstOrDefaultAsync(d => d.DonationId == donation.DonationId);
    }
    public async Task SaveChangesAsync() => await _ctx.SaveChangesAsync();
}