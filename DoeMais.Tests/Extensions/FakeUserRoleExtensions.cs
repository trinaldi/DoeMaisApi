using DoeMais.Domain.Entities;
using DoeMais.Tests.Domain;

public static class FakeUserRoleExtensions
{
    public static UserRole ToUserRole(this FakeUserRole fakeUserRole)
    {
        return new UserRole
        {
            UserId = fakeUserRole.UserId,
            RoleId = fakeUserRole.RoleId,
            Role = null,
            User = null
        };
    }
}