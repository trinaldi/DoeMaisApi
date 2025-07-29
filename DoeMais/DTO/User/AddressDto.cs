namespace DoeMais.DTO.User;

public class AddressDto
{
    public long AddressId { get; set; }
    public string Street { get; set; } = "";
    public string? Complement { get; set; }
    public string Neighborhood { get; set; } = "";
    public string City { get; set; } = "";
    public string State { get; set; } = "";
    public string ZipCode { get; set; } = "";
    public bool IsPrimary { get; set; } 
}