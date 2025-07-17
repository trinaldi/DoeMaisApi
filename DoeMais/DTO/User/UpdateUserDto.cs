using System.ComponentModel.DataAnnotations;

namespace DoeMais.DTO.User;

public class UpdateUserDto
{
    [Url(ErrorMessage = "AvatarUrl must be a valid URL.")]
    public string? AvatarUrl { get; set; }

    [StringLength(100, ErrorMessage = "Name must be at most 100 characters.")]
    public string? Name { get; set; } = "";

    [RegularExpression(@"^\d{11}$", ErrorMessage = "Phone number must be exactly 11 digits, e.g. 99999999999.")]
    public string? Phone { get; set; } = "";

    [StringLength(200, ErrorMessage = "Address must be at most 200 characters.")]
    public string? Address { get; set; } = "";

    [StringLength(100, ErrorMessage = "Complement must be at most 100 characters.")]
    public string? Complement { get; set; }

    [StringLength(100, ErrorMessage = "Neighborhood must be at most 100 characters.")]
    public string? Neighborhood { get; set; } = "";

    [StringLength(100, ErrorMessage = "City must be at most 100 characters.")]
    public string? City { get; set; } = "";

    [StringLength(2, MinimumLength = 2, ErrorMessage = "State must be exactly 2 characters.")]
    public string? State { get; set; } = "";

    [RegularExpression(@"^\d{5}-?\d{3}$", ErrorMessage = "ZipCode format is invalid. Expected format: 12345-678 or 12345678")]
    public string? ZipCode { get; set; } = "";
}
