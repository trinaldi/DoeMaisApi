using Microsoft.AspNetCore.Mvc;
using DoeMais.Models;
using DoeMais.Data;
using DoeMais.DTOs;
using DoeMais.Extensions;
using DoeMais.Services.Utils;
using DoeMais.Services.Interfaces;
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
            return BadRequest("Usuário já existe");

        var user = dto.ToUser(_passwordHasher);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("Usuário registrado");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null || !_passwordHasher.VerifyHashedPassword(dto.Password, user.PasswordHash))
            return Unauthorized("Credenciais inválidas");

        var token = _tokenGeneratorService.GenerateToken(user);
        return Ok(new { token });
    }
}