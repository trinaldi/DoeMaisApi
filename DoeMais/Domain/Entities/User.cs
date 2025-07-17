namespace DoeMais.Domain.Entities;

public class User
{
    public Int64 UserId { get; set; }
    public string? AvatarUrl { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = "";
    public string Cpf { get; set; } = "";
    public string Address { get; set; } = "";
    public string? Complement { get; set; }
    public string Neighborhood { get; set; } = "";
    public string City { get; set; } = "";
    public string State { get; set; } = "";
    public string ZipCode { get; set; } = "";
    public string PasswordHash { get; set; } = default!;
    public string Role { get; set; } = "Donor";
}