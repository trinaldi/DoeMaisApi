using DoeMais.Common;
using DoeMais.Domain.Enums;
using DoeMais.DTO.Address;
using DoeMais.Extensions;
using DoeMais.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoeMais.Controllers.Address;

[ApiController]
[Route("[controller]")]
public class AddressController : ControllerBase
{
    private readonly IAddressService _addressService;

    public AddressController(IAddressService addressService)
    {
        _addressService = addressService;
    }

    [Authorize]
    [HttpGet("all")]
    public async Task<IActionResult> GetAddresses()
    {
        var userId = User.GetUserId();
        var result = await _addressService.GetAddressesAsync(userId);

        return result.Type switch
        {
            ResultType.Success => Ok(result.Data),
            _ => BadRequest("Something went wrong.")
        };
    }
    
    [Authorize]
    [HttpGet("{addressId:long}")]
    public async Task<IActionResult> GetAddressById(long addressId)
    {
        var userId = User.GetUserId();
        var result = await _addressService.GetAddressByIdAsync(addressId, userId);

        return result.Type switch
        {
            ResultType.Success => Ok(result.Data),
            ResultType.NotFound => NotFound(result.Message),
            _ => BadRequest("Something went wrong.")
        };
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateAddress([FromBody] AddressDto dto)
    {
        var userId = User.GetUserId();
        var result = await _addressService.CreateAddressAsync(dto, userId);

        return result.Type switch
        {
            ResultType.Success => Ok(result.Data),
            ResultType.Error => BadRequest(result.Message),
            _ => BadRequest("Something went wrong.")
        };
    }
    
    [Authorize]
    [HttpPut("{addressId:long}")]
    public async Task<IActionResult> UpdateAddress(long addressId, [FromBody]AddressDto dto)
    {
        var userId = User.GetUserId();
        var result = await _addressService.UpdateAddressAsync(addressId, dto, userId);

        return result.Type switch
        {
            ResultType.Success => Ok(result.Data),
            ResultType.Error => BadRequest(result.Message),
            ResultType.Mismatch => NotFound(result.Message),
            _ => BadRequest("Something went wrong.")
        };

    }

    [Authorize]
    [HttpDelete("{addressId:long}")]
    public async Task<IActionResult> DeleteAddress(long addressId)
    {
        var userId = User.GetUserId();
        var result = await _addressService.DeleteAddressAsync(addressId, userId);

        return result.Type switch
        {
            ResultType.Success => NoContent(),
            ResultType.Error => NotFound(result.Message),
            _ => BadRequest("Something went wrong.")
        };
    }
}