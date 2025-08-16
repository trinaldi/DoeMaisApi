using System.ComponentModel.DataAnnotations;

namespace DoeMais.DTOs.Auth;

public record LoginUserDto
{
    [Required]
    [EmailAddress]
    public string Email { get; init; } = null!;
    [Required]
    public string Password { get; init; } = null!;
}