using DoeMais.Common;
using DoeMais.Domain.Enums;
using DoeMais.DTOs.Address;
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
    public async Task<IActionResult> GetAll()
    {
        var result = await _addressService.GetAddressesAsync();

        return result.Type switch
        {
            ResultType.Success => Ok(result),
            _ => BadRequest("Something went wrong.")
        };
    }
    
    [Authorize]
    [HttpGet("{addressId:long}")]
    public async Task<IActionResult> GetById(long addressId)
    {
        var result = await _addressService.GetAddressByIdAsync(addressId);

        return result.Type switch
        {
            ResultType.Success => Ok(result),
            ResultType.NotFound => NotFound(result),
            _ => BadRequest("Something went wrong.")
        };
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddressDto dto)
    {
        var result = await _addressService.CreateAddressAsync(dto);

        return result.Type switch
        {
            ResultType.Success => Ok(result),
            ResultType.Error => BadRequest(result),
            _ => BadRequest("Something went wrong.")
        };
    }
    
    [Authorize]
    [HttpPut("{addressId:long}")]
    public async Task<IActionResult> Update(long addressId, [FromBody]AddressDto dto)
    {
        var result = await _addressService.UpdateAddressAsync(addressId, dto);

        return result.Type switch
        {
            ResultType.Success => Ok(result),
            ResultType.Error => BadRequest(result),
            ResultType.Mismatch => NotFound(result),
            _ => BadRequest("Something went wrong.")
        };

    }

    [Authorize]
    [HttpDelete("{addressId:long}")]
    public async Task<IActionResult> Delete(long addressId)
    {
        var result = await _addressService.DeleteAddressAsync(addressId);

        return result.Type switch
        {
            ResultType.Success => Ok(result),
            ResultType.Error => NotFound(result),
            _ => BadRequest("Something went wrong.")
        };
    }
}