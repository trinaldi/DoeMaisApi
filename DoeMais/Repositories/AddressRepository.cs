using DoeMais.Data;
using DoeMais.Domain.Entities;
using DoeMais.Extensions;
using DoeMais.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DoeMais.Repositories;

public class AddressRepository : IAddressRepository
{
    private readonly AppDbContext _ctx;

    public AddressRepository(AppDbContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<List<Address>?> GetAddressesAsync(long userId)
    {
        return await _ctx.Addresses
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.IsPrimary)
            .ToListAsync();
    }

    public async Task<Address?> GetAddressByIdAsync(long addressId, long userId)
    {
        return await _ctx.Addresses
            .Where(a => a.UserId == userId && a.AddressId == addressId)
            .FirstOrDefaultAsync();
    }

    public async Task<Address?> CreateAddressAsync(Address address, long userId)
    {
        var user =  await _ctx.Users
            .Include(u => u.Addresses)
            .FirstOrDefaultAsync(u => u.UserId == userId);
        if (user is null) return null;

        user.Addresses.Add(address);
        await _ctx.SaveChangesAsync();
        
        return address;
    }

    public async Task<Address?> UpdateAddressAsync(long addressId, Address address, long userId)
    {
        var user = await _ctx.Users
            .Include(u => u.Addresses)
            .FirstOrDefaultAsync(u => u.UserId == userId);
        if (user is null) return null;

        var existingAddress = user.Addresses.FirstOrDefault(a => a.AddressId == addressId);
        if (existingAddress is null) return null;
        
        existingAddress.UpdateFrom(address);
        await _ctx.SaveChangesAsync();
        
        return existingAddress;
    }

    public async Task<bool> DeleteAddressAsync(long addressId, long userId)
    {
        var address = await _ctx.Addresses
            .Where(d => d.AddressId == addressId && d.UserId == userId)
            .FirstOrDefaultAsync();
        if (address == null) return false;
        
        _ctx.Addresses.Remove(address);
        await _ctx.SaveChangesAsync();
        
        return true;

    }
}