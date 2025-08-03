namespace DoeMais.DTO.Address;

public record AddressDto
{
    public long AddressId { get; init; }
    public string? Street { get; init; }
    public string? Complement { get; init; }
    public string? Neighborhood { get; init; }
    public string? City { get; init; }
    public string? State { get; init; }
    public string? ZipCode { get; init; }
    public bool IsPrimary { get; init; } 
}