using DoeMais.Common;
using DoeMais.Domain.Enums;
using DoeMais.DTOs.Address;
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
    public async Task<Result<List<AddressDto>?>> GetAddressesAsync()
    {
        var addresses = await _addressRepository.GetAddressesAsync();
        var result = addresses?.Select(a => a.ToDto()).ToList();
        
        return new Result<List<AddressDto>?>(ResultType.Success, result);
    }

    public async Task<Result<AddressDto>> GetAddressByIdAsync(long addressId) { 
        var address = await _addressRepository.GetAddressByIdAsync(addressId);
        return address == null
            ? new Result<AddressDto>(ResultType.NotFound, null, "Address not found.") 
            : new Result<AddressDto>(ResultType.Success, address.ToDto());
    }

    public async Task<Result<AddressDto?>> CreateAddressAsync(AddressDto dto)
    {
        if(string.IsNullOrWhiteSpace(dto.Street))
            throw new ArgumentNullException(nameof(dto.Street), "Street is required.");

        var address = dto.ToEntity();
        var result = await _addressRepository.CreateAddressAsync(address);
        return result == null
            ? new Result<AddressDto?>(ResultType.Error, null, "Address not created.")
            : new Result<AddressDto?>(ResultType.Success, address.ToDto());
    }

    public async Task<Result<AddressDto?>> UpdateAddressAsync(long addressId, AddressDto dto)
    {
        if (dto.AddressId != addressId) return new Result<AddressDto?>(ResultType.Mismatch, null, "Address ids don't match.");
        var address = dto.ToEntity();
        var result = await _addressRepository.UpdateAddressAsync(addressId, address);
        
        return result == null 
            ? new Result<AddressDto?>(ResultType.Error, null, "Address not updated.") 
            : new Result<AddressDto?>(ResultType.Success, address.ToDto());
    }

    public async Task<Result<bool>> DeleteAddressAsync(long addressId)
    {
        var success = await _addressRepository.DeleteAddressAsync(addressId);
        return success ? new Result<bool>(ResultType.Success, success) : new Result<bool>(ResultType.NotFound, success); 
    }
}