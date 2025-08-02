using DoeMais.DTO.Address;
using DoeMais.Extensions;
using DoeMais.Repositories.Interfaces;
using DoeMais.Services.Interfaces;

namespace DoeMais.Services;

public class AddressService : IAddressService
{
    private readonly IAddressRepository _addressRepository;

    public AddressService(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }
    public async Task<List<AddressDto>?> GetAddressesAsync(long userId)
    {
        var addresses = await _addressRepository.GetAddressesAsync(userId);
        var result = addresses?.Select(a => a.ToDto()).ToList();
        
        return result;
    }

    public async Task<AddressDto?> GetAddressByIdAsync(long addressId, long userId)
    {
        var address = await _addressRepository.GetAddressByIdAsync(addressId, userId);
        
        return address?.ToDto();
    }

    public async Task<AddressDto?> CreateAddressAsync(AddressDto dto, long userId)
    {
        var address = dto.ToEntity();
        address.UserId = userId;
        var result = await _addressRepository.CreateAddressAsync(address, address.UserId);
        
        return result?.ToDto();
    }

    public async Task<AddressDto?> UpdateAddressAsync(long addressId, AddressDto dto, long userId)
    {
        if (dto.AddressId != addressId) return null;
        var address = dto.ToEntity();
        var result = await _addressRepository.UpdateAddressAsync(addressId, address, userId);
        
        return result?.ToDto();
    }

    public async Task<bool> DeleteAddressAsync(long addressId, long userId)
    {
        var success = await _addressRepository.DeleteAddressAsync(addressId, userId);
        
        return success;
    }
}