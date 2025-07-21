namespace DoeMais.DTO.User;

using System.ComponentModel.DataAnnotations;

public class UserProfileDto
{
    public string? AvatarUrl { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name must be at most 100 characters.")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Phone is required.")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "Phone number must be exactly 11 digits, e.g. 99999999999.")]
    public string Phone { get; set; } = "";

    [Required(ErrorMessage = "Address is required.")]
    [StringLength(200, ErrorMessage = "Address must be at most 200 characters.")]
    public string Address { get; set; } = "";

    [StringLength(100, ErrorMessage = "Complement must be at most 100 characters.")]
    public string? Complement { get; set; }

    [Required(ErrorMessage = "Neighborhood is required.")]
    [StringLength(100, ErrorMessage = "Neighborhood must be at most 100 characters.")]
    public string Neighborhood { get; set; } = "";

    [Required(ErrorMessage = "City is required.")]
    [StringLength(100, ErrorMessage = "City must be at most 100 characters.")]
    public string City { get; set; } = "";

    [Required(ErrorMessage = "State is required.")]
    [StringLength(2, MinimumLength = 2, ErrorMessage = "State must be exactly 2 characters.")]
    public string State { get; set; } = "";

    [Required(ErrorMessage = "ZipCode is required.")]
    [RegularExpression(@"^\d{5}-?\d{3}$", ErrorMessage = "ZipCode format is invalid. Expected format: 12345-678 or 12345678")]
    public string ZipCode { get; set; } = "";
}
