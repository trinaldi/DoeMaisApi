using DoeMais.Domain.Interfaces;

namespace DoeMais.Domain.Entities;

public class UserRole : IUserOwned
{
    public long UserId { get; set; }
    public User User { get; set; } = null!;
    
    public long RoleId { get; set; }
    public Role Role { get; set; } = null!;
}