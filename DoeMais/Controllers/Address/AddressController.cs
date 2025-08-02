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
        var addresses = await _addressService.GetAddressesAsync(userId);
        
        return Ok(addresses);
    }
    
    [Authorize]
    [HttpGet("{addressId:long}")]
    public async Task<IActionResult> GetAddressById(long addressId)
    {
        var userId = User.GetUserId();
        var address = await _addressService.GetAddressByIdAsync(addressId, userId);
        
        return Ok(address);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateAddress([FromBody] AddressDto dto)
    {
        var userId = User.GetUserId();
        var address = await _addressService.CreateAddressAsync(dto, userId);
        
        return Ok(address);
    }
    
    [Authorize]
    [HttpPut("{addressId:long}")]
    public async Task<IActionResult> UpdateAddress(long addressId, [FromBody]AddressDto dto)
    {
        var userId = User.GetUserId();
        var address = await _addressService.UpdateAddressAsync(addressId, dto, userId);
        
        return Ok(address);
    }

    [Authorize]
    [HttpDelete("{addressId:long}")]
    public async Task<IActionResult> DeleteAddress(long addressId)
    {
        var userId = User.GetUserId();
        var success = await _addressService.DeleteAddressAsync(addressId, userId);
        if (!success) return NotFound($"Address {addressId} not found.");
        
        return NoContent();
    }
}