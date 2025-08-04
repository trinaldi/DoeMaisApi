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
        var userId = User.GetUserId();
        var donation = await _donationService.CreateDonationAsync(dto, userId);
        if (donation is null) return BadRequest("Error while creating donation");

        return Ok(donation);
    }

    [Authorize]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetDonationById(long id)
    {
        var userId = User.GetUserId();
        var donation = await _donationService.GetDonationByIdAsync(id, userId);
        if (donation is null) return NotFound(new { message = $"Donation {id} not found." });
        
        return Ok(donation);
    }
    
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetDonationList()
    {
        var userId = User.GetUserId();
        var donations = await _donationService.GetDonationListAsync(userId);
        
        return Ok(donations);
    }


    [Authorize]
    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateDonationAsync(long id, [FromBody] UpdateDonationDto dto)
    {
        var userId = User.GetUserId();
        var updatedDonation = await _donationService.UpdateDonationAsync(dto, userId);

        return Ok(updatedDonation);
    }
    
    [Authorize]
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteDonation(long id)
    {
        var userId = User.GetUserId();
        var success = await _donationService.DeleteDonationAsync(id, userId);
        if (!success) return NotFound($"Donation {id} not found.");
        
        return NoContent();
    }
}