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

    public List<AddressDto> Addresses { get; set; } = [];
}