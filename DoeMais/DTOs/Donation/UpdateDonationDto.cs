using System.ComponentModel.DataAnnotations;
using DoeMais.Domain.Enums;

namespace DoeMais.DTOs.Donation;

public record UpdateDonationDto
{
    public long AddressId { get; init; }
    public long UserId { get; init; }
    public long DonationId { get; init; }
    
    public string? Title { get; init; } 
    public string? Description { get; init; } 
    public int? Quantity { get; init; } 
    public Status? Status { get; init; }
    [Range(1, 10, ErrorMessage = "Categoria inválida. Digite 10 para 'Outros'.")]
    public Category? Category { get; init; }
}