using System.ComponentModel.DataAnnotations;
using DoeMais.Domain.OwnedTypes;
using DoeMais.Domain.Enums;
using DoeMais.DTOs.Address;

namespace DoeMais.DTOs.Donation;

public record CreateDonationDto
{
    public long UserId { get; init; }
    
    [Required]
    [StringLength(100, ErrorMessage = "Title must be at most 100 characters.")]
    public required string Title { get; init; } 
    
    [StringLength(500, ErrorMessage = "Description must be at most 500 characters." )]
    public string? Description { get; init; }
    public int? Quantity { get; init; } = 1;
    public Status Status { get; init; } = Status.Pending;
    [Required]
    [Range(1, 10, ErrorMessage = "Categoria inválida. Digite 10 para 'Outros'.")]
    public Category Category { get; init; }
    public AddressDto PickupAddress { get; init; }
    public AddressDto DeliveryAddress { get; init; }
    public ICollection<string>? Images { get; init; } = new List<string>();
}