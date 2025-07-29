using System.ComponentModel.DataAnnotations;
using DoeMais.Domain.ValueObjects;

namespace DoeMais.DTO.Auth;

public class RegisterUserDto
{
    [Required]
    public string Name { get; set; } = default!;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string? Cpf { get; set; }
}