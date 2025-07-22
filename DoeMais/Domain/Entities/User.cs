namespace DoeMais.Domain.Entities;

public class User
{
    public long UserId { get; set; }
    public string? AvatarUrl { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = "";
    public string Cpf { get; set; } = "";
    public List<Address> Addresses { get; set; } = [];
    public string PasswordHash { get; set; } = default!;
    
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}