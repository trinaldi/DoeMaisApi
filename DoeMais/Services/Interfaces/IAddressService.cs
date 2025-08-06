using DoeMais.Common;
using DoeMais.DTO.Address;

namespace DoeMais.Services.Interfaces;

public interface IAddressService
{
    Task<Result<List<AddressDto>?>> GetAddressesAsync();
    Task<Result<AddressDto>> GetAddressByIdAsync(long addressId);
    Task<Result<AddressDto?>> CreateAddressAsync(AddressDto dto);
    Task<Result<AddressDto?>> UpdateAddressAsync(long addressId, AddressDto dto);
    Task<Result<bool>> DeleteAddressAsync(long addressId);
}