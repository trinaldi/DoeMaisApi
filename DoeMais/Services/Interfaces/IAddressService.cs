using DoeMais.DTO.Address;

namespace DoeMais.Services.Interfaces;

public interface IAddressService
{
    Task<List<AddressDto>?> GetAddressesAsync(long userId);
    Task<AddressDto?> GetAddressByIdAsync(long addressId, long userId);
    Task<AddressDto?> CreateAddressAsync(AddressDto dto, long userId);
    Task<AddressDto?> UpdateAddressAsync(long addressId, AddressDto dto, long userId);
    Task<bool> DeleteAddressAsync(long addressId, long userId);
}