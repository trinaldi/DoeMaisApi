using System.ComponentModel.DataAnnotations;
using DoeMais.DTO.Address;

namespace DoeMais.DTO.User;

public class UpdateUserDto
{
    public string? AvatarUrl { get; set; }

    [StringLength(100, ErrorMessage = "Name must be at most 100 characters.")]
    public string? Name { get; set; } = "";

    [RegularExpression(@"^\d{11}$", ErrorMessage = "Phone number must be exactly 11 digits, e.g. 99999999999.")]
    public string? Phone { get; set; } = "";

    public ICollection<AddressDto> Addresses { get; set; } = [];
}
