using DoeMais.Common;
using DoeMais.DTO.Address;

namespace DoeMais.Services.Interfaces;

public interface IAddressService
{
    Task<Result<List<AddressDto>?>> GetAddressesAsync(long userId);
    Task<Result<AddressDto>> GetAddressByIdAsync(long addressId, long userId);
    Task<Result<AddressDto?>> CreateAddressAsync(AddressDto dto, long userId);
    Task<Result<AddressDto?>> UpdateAddressAsync(long addressId, AddressDto dto, long userId);
    Task<Result<bool>> DeleteAddressAsync(long addressId, long userId);
}