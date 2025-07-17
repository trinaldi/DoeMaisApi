using System.ComponentModel.DataAnnotations;

namespace DoeMais.DTO.Auth;

public class RegisterUserDto
{
    [Required]
    public string Name { get; set; } = default!;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Role { get; set; } = "Donor";
}