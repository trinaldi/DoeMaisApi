using DoeMais.Domain.Enums;
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
        var result = await _userService.GetByIdAsync(userId);

        return result.Type switch
        {
            ResultType.Success => Ok(result),
            ResultType.NotFound => NotFound(result),
            _ => StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong"),
        };

    }

    [Authorize]
    [HttpPut("me")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserDto dto)
    {
        var userId = User.GetUserId();
        var result = await _userService.UpdateUserAsync(userId, dto);
           
        return result.Type switch
        {
            ResultType.Success => Ok(result),
            ResultType.NotFound => NotFound(result),
            _ => StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong"),
        };
    }
}
