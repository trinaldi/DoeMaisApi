using DoeMais.DTOs.Address;

namespace DoeMais.DTOs.User;

using System.ComponentModel.DataAnnotations;

public record UserProfileDto
{
    public long UserId { get; init; }
    public string? AvatarUrl { get; init; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name must be at most 100 characters.")]
    public string Name { get; init; } = "";

    [Required(ErrorMessage = "Phone is required.")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "Phone number must be exactly 11 digits, e.g. 99999999999.")]
    public string Phone { get; init; } = "";

    public AddressDto? AddressDto { get; set; }
    
    public override string ToString() => 
        $"AvatarUrl: {AvatarUrl}, Name: {Name}, Phone: {Phone}, Address: {AddressDto}";
}