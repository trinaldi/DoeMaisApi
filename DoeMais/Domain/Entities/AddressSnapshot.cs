using Microsoft.EntityFrameworkCore;

namespace DoeMais.Domain.Entities;

public record AddressSnapshot
{
    public string Street { get; init; } = "";
    public string? Complement { get; init; }
    public string Neighborhood { get; init; } = "";
    public string City { get; init; } = "";
    public string State { get; init; } = "";
    public string ZipCode { get; init; } = ""; 
}