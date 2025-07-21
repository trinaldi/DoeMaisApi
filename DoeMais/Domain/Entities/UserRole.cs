namespace DoeMais.Domain.Entities;

public class UserRole
{
    public Int64 UserId { get; set; }
    public User User { get; set; } = null!;
    
    public Int64 RoleId { get; set; }
    public Role Role { get; set; } = null!;
}