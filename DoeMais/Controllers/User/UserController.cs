using DoeMais.Data;
using DoeMais.DTO.User;
using DoeMais.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoeMais.Controllers.User;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;

    public UserController(AppDbContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var userId = User.GetUserId();
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        if (user is null) return NotFound();

        var userProfileDto = user.ToDto();
        
        return Ok(userProfileDto);
    }
    [Authorize]
    [HttpPut("me")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserDto dto)
    {
        var userId = User.GetUserId();

        var user = await _context.Users.FindAsync(userId);
        if (user is null) return NotFound();

        user.UpdateFromDto(dto);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
