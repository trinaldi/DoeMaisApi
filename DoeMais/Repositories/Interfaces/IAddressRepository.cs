using DoeMais.Domain.Entities;

namespace DoeMais.Repositories.Interfaces;

public interface IAddressRepository
{
    Task<List<Address>?> GetAddressesAsync(long userId);
    
    Task<Address?> GetAddressByIdAsync(long addressId, long userId);
    Task<Address?> CreateAddressAsync(Address address, long userId);
    Task<Address?> UpdateAddressAsync(long addressId, Address address, long userId);
    Task<bool> DeleteAddressAsync(long addressId, long userId);
}