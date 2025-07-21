using DoeMais.Domain.Entities;
using DoeMais.Tests.Domain;

namespace DoeMais.Tests.Extensions;

public static class FakeRoleExtensions
{
    public static Role ToRole(this FakeRole fakeRole)
    {
        return new Role(fakeRole.RoleId, fakeRole.Name);
    }
}