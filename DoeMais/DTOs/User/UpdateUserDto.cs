using System.ComponentModel.DataAnnotations;
using DoeMais.DTOs.Address;

namespace DoeMais.DTOs.User;

public record UpdateUserDto
{
    public string? AvatarUrl { get; init; }

    [StringLength(100, ErrorMessage = "Name must be at most 100 characters.")]
    public string? Name { get; init; } = "";

    [RegularExpression(@"^\d{11}$", ErrorMessage = "Phone number must be exactly 11 digits, e.g. 99999999999.")]
    public string? Phone { get; init; } = "";

    public ICollection<AddressDto> Addresses { get; init; } = [];
}
