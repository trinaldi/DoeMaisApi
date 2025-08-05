using DoeMais.Domain.Enums;
using DoeMais.DTO.Donation;
using DoeMais.Extensions;
using DoeMais.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoeMais.Controllers.Donation;

[ApiController]
[Route("[controller]")]
public class DonationController : ControllerBase
{
    private readonly IDonationService _donationService;
    public DonationController(IDonationService donationService)
    {
        _donationService = donationService;
    }
    
    [Authorize]
    [HttpPost("me")]
    public async Task<IActionResult> CreateDonationForMe([FromBody] CreateDonationDto dto)
    {
        var result = await _donationService.CreateDonationAsync(dto);

        return result.Type switch
        {
            ResultType.Error => BadRequest("Error while creating donation"),
            ResultType.Success => Ok(result),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }

    [Authorize]
    [HttpGet("{donationId:long}")]
    public async Task<IActionResult> GetDonationById(long donationId)
    {
        var result = await _donationService.GetDonationByIdAsync(donationId);
        
        return result.Type switch
        {
            ResultType.NotFound => NotFound("Donation not found."),
            ResultType.Success => Ok(result),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
    
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetDonationList()
    {
        var result = await _donationService.GetDonationListAsync();
        
        return result?.Type switch
        {
            ResultType.Success => Ok(result),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }


    [Authorize]
    [HttpPut("{donationId:long}")]
    public async Task<IActionResult> UpdateDonationAsync(long donationId, [FromBody] UpdateDonationDto dto)
    {
        if (donationId != dto.DonationId)
            return BadRequest();
        
        var result = await _donationService.UpdateDonationAsync(dto);
        
        return result.Type switch
        {
            ResultType.NotFound => NotFound(result),
            ResultType.Error => NotFound(result),
            ResultType.Success => Ok(result),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
    
    [Authorize]
    [HttpDelete("{donationId:long}")]
    public async Task<IActionResult> DeleteDonation(long donationId)
    {
        var result = await _donationService.DeleteDonationAsync(donationId);
        
        return result.Type switch
        {
            ResultType.Error => BadRequest(result),
            ResultType.Success => Ok(result),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}