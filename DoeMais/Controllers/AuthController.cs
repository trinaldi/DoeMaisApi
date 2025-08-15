using Microsoft.AspNetCore.Mvc;
using DoeMais.DTOs.Auth;
using DoeMais.Extensions;
using DoeMais.Infrastructure;
using DoeMais.Services.Utils;
using DoeMais.Services.Interfaces.Utils;
using Microsoft.EntityFrameworkCore;

namespace DoeMais.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly TokenGeneratorService _tokenGeneratorService;
    private readonly IPasswordHasher _passwordHasher;

    public AuthController(AppDbContext context, TokenGeneratorService tokenGeneratorService, IPasswordHasher passwordHasher)
    {
        _context = context;
        _tokenGeneratorService = tokenGeneratorService;
        _passwordHasher = passwordHasher;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDto dto)
    {
        if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            return BadRequest("User already exists");

        try
        {
            var user = dto.ToUser(_passwordHasher);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Created();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserDto dto)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == dto.Email);
        
        if (user == null || !_passwordHasher.VerifyHashedPassword(dto.Password, user.PasswordHash))
            return Unauthorized("Wrong credentials");

        var token = _tokenGeneratorService.GenerateToken(user);
        return Ok(new { token });
    }
}