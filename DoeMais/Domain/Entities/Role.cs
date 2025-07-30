namespace DoeMais.Domain.Entities;

public class Role
{
    public long RoleId { get; set; }
    public string Name { get; private set; }

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    protected Role() { }

    public Role(long roleId, string name) : this(name)
    {
        RoleId = roleId;
    }
    
    public Role(string name)
    {
        if(String.IsNullOrEmpty(name))
            throw new ArgumentNullException(nameof(name), "Role name cannot be null or empty.");
        Name = name;
    }
}