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
    
    public async Task<List<Address>?> GetAddressesAsync()
    {
        return await _ctx.Addresses
            .OrderByDescending(a => a.IsPrimary)
            .ToListAsync();
    }

    public async Task<Address?> GetAddressByIdAsync(long addressId)
    {
        return await _ctx.Addresses
            .Where(a => a.AddressId == addressId)
            .FirstOrDefaultAsync();
    }

    public async Task<Address> CreateAddressAsync(Address address)
    {
        _ctx.Addresses.Add(address);
        await _ctx.SaveChangesAsync();
        
        return address;
    }

    public async Task<Address?> UpdateAddressAsync(long addressId, Address address)
    {
        var existingAddress = await _ctx.Addresses
            .FirstOrDefaultAsync(a => a.AddressId == addressId);
        if (existingAddress is null) return null;
        
        existingAddress.UpdateFrom(address);
        await _ctx.SaveChangesAsync();
        
        return existingAddress;
    }

    public async Task<bool> DeleteAddressAsync(long addressId)
    {
        var address = await _ctx.Addresses
            .Where(d => d.AddressId == addressId)
            .FirstOrDefaultAsync();
        if (address == null) return false;
        
        _ctx.Addresses.Remove(address);
        await _ctx.SaveChangesAsync();
        
        return true;
    }
}