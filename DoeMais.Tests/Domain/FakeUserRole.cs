using DoeMais.Domain.Entities;

namespace DoeMais.Tests.Domain;

public class FakeUserRole
{
    public long UserId { get; set; }
    public FakeUser User { get; set; } = null!;
    
    public long RoleId { get; set; }
    public FakeRole Role { get; set; } = null!;
    
    public static FakeUserRole Create(FakeUser user, FakeRole? role = null)
    {
        var fakeRole = role ?? FakeRole.Create();

        var fakeUserRole =  new FakeUserRole
        {
            User = user,
            UserId = user.UserId,
            Role = fakeRole,
            RoleId = fakeRole.RoleId
        };
        
        fakeRole.FakeUserRoles.Add(fakeUserRole);
        
        return fakeUserRole;
    }
}