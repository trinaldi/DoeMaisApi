using System.ComponentModel.DataAnnotations;

namespace DoeMais.DTO.Auth;

public class LoginUserDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;
    [Required]
    public string Password { get; set; } = default!;
}