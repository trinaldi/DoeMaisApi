using DoeMais.Domain.ValueObjects;

namespace DoeMais.Domain.Entities;

public class User
{
    public long UserId { get; set; }
    public string? AvatarUrl { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = "";
    public Cpf Cpf { get; set; }
    public List<Address> Addresses { get; set; } = [];
    public List<Donation> Donations { get; set; } = [];
    public string PasswordHash { get; set; } = default!;
    
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    
    private string CpfValue
    {
        get => Cpf.Value;
        set => Cpf = new Cpf(value);
    }
}