using DoeMais.Domain.Enums;
using DoeMais.DTOs.Donation;
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
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDonationDto dto)
    {
        var result = await _donationService.CreateDonationAsync(dto);

        return result.Type switch
        {
            ResultType.Error => BadRequest("Error while creating donation."),
            ResultType.Success => Ok(result),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }

    [Authorize]
    [HttpGet("{donationId:long}")]
    public async Task<IActionResult> GetById(long donationId)
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
    public async Task<IActionResult> GetAll()
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
    public async Task<IActionResult> Update(long donationId, [FromBody] UpdateDonationDto dto)
    {
        var result = await _donationService.UpdateDonationAsync(donationId, dto);
        
        return result.Type switch
        {
            ResultType.Mismatch => BadRequest(result),
            ResultType.NotFound => NotFound(result),
            ResultType.Error => NotFound(result),
            ResultType.Success => Ok(result),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
    
    [Authorize]
    [HttpDelete("{donationId:long}")]
    public async Task<IActionResult> Delete(long donationId)
    {
        var result = await _donationService.DeleteDonationAsync(donationId);
        
        return result.Type switch
        {
            ResultType.Error => NotFound(result),
            ResultType.Success => Ok(result),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}