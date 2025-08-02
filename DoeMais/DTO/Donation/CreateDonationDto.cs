using System.ComponentModel.DataAnnotations;
using DoeMais.Domain.Enums;

namespace DoeMais.DTO.Donation;

public class CreateDonationDto
{
    public long AddressId { get; set; }
    
    [Required]
    [StringLength(100, ErrorMessage = "Title must be at most 100 characters.")]
    public required string Title { get; set; } 
    
    public string? Description { get; set; }
    public int? Quantity { get; set; } = 1;
    public Status Status { get; set; } = Status.Pending;
    public ICollection<string>? Images { get; set; } = new List<string>();
}