using DoeMais.Domain.Entities;

namespace DoeMais.Repositories.Interfaces;

public interface IAddressRepository
{
    Task<List<Address>?> GetAddressesAsync();
    
    Task<Address?> GetAddressByIdAsync(long addressId);
    Task<Address> CreateAddressAsync(Address address);
    Task<Address?> UpdateAddressAsync(long addressId, Address address);
    Task<bool> DeleteAddressAsync(long addressId);

    Task ClearPrimaryAddressAsync();
}