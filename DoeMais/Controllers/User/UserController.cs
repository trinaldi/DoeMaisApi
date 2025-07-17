using DoeMais.DTO.User;
using DoeMais.Exceptions;
using DoeMais.Extensions;
using DoeMais.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoeMais.Controllers.User;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var userId = User.GetUserId();
        var user = await _userService.GetByIdAsync(userId);
        if (user is null) return NotFound();

        var userProfileDto = user.ToDto();
        
        return Ok(userProfileDto);
    }
    
    [Authorize]
    [HttpPut("me")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserDto dto)
    {
        var userId = User.GetUserId();
        try
        {
            var updatedUser = await _userService.UpdateUserAsync(userId, dto);
            var updatedUserDto = updatedUser?.ToUpdateUserDto();
        
            return Ok(updatedUserDto);
        }
        catch (NotFoundException<Domain.Entities.User> e)
        {
            return NotFound(e.Message);
        }
    }
}
