using System.ComponentModel.DataAnnotations;
using DoeMais.Domain.Enums;

namespace DoeMais.DTO.Donation;

public record CreateDonationDto
{
    public long UserId { get; init; }
    public long AddressId { get; init; }
    
    [Required]
    [StringLength(100, ErrorMessage = "Title must be at most 100 characters.")]
    public required string Title { get; init; } 
    
    public string? Description { get; init; }
    public int? Quantity { get; init; } = 1;
    public Status Status { get; init; } = Status.Pending;
    public ICollection<string>? Images { get; init; } = new List<string>();
}