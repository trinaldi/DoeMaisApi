using System.ComponentModel.DataAnnotations;
using DoeMais.Domain.ValueObjects;

namespace DoeMais.DTO.Auth;

public record RegisterUserDto
{
    [Required]
    public string Name { get; init; } = null!;
    [Required]
    [EmailAddress]
    public string Email { get; init; } = null!;
    [Required]
    public string Password { get; init; } = null!;
    [Required]
    public string Cpf { get; init; }
}