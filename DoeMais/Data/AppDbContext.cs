using Microsoft.EntityFrameworkCore;
using DoeMais.Domain.User;

namespace DoeMais.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
}