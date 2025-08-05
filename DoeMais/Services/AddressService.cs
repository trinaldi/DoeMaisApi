using DoeMais.Common;
using DoeMais.Domain.Enums;
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
    public async Task<Result<List<AddressDto>?>> GetAddressesAsync(long userId)
    {
        var addresses = await _addressRepository.GetAddressesAsync(userId);
        var result = addresses?.Select(a => a.ToDto()).ToList();
        
        return new Result<List<AddressDto>?>(ResultType.Success, result);
    }

    public async Task<Result<AddressDto>> GetAddressByIdAsync(long addressId, long userId) { 
        var address = await _addressRepository.GetAddressByIdAsync(addressId, userId);
        return address == null
            ? new Result<AddressDto>(ResultType.NotFound, null, "Address not found.") 
            : new Result<AddressDto>(ResultType.Success, address.ToDto());
    }

    public async Task<Result<AddressDto?>> CreateAddressAsync(AddressDto dto, long userId)
    {
        var address = dto.ToEntity();
        address.UserId = userId;
        var result = await _addressRepository.CreateAddressAsync(address, address.UserId);
        return result == null
            ? new Result<AddressDto?>(ResultType.Error, null, "Address not created.")
            : new Result<AddressDto?>(ResultType.Success, address.ToDto());
    }

    public async Task<Result<AddressDto?>> UpdateAddressAsync(long addressId, AddressDto dto, long userId)
    {
        if (dto.AddressId != addressId) return new Result<AddressDto?>(ResultType.Mismatch, null, "Address ids don't match.");
        var address = dto.ToEntity();
        var result = await _addressRepository.UpdateAddressAsync(addressId, address, userId);
        
        return result == null 
            ? new Result<AddressDto?>(ResultType.Error, null, "Address not updated.") 
            : new Result<AddressDto?>(ResultType.Success, address.ToDto());
    }

    public async Task<Result<bool>> DeleteAddressAsync(long addressId, long userId)
    {
        var success = await _addressRepository.DeleteAddressAsync(addressId, userId);
        return success ? new Result<bool>(ResultType.Success, success) : new Result<bool>(ResultType.NotFound, success); 
    }
}