using Microsoft.EntityFrameworkCore;
using DoeMais.Domain.Entities;
using DoeMais.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DoeMais.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(50);
        });
        
        var cpfConverter = new ValueConverter<Cpf, string>(
            v => v.Value,
            v => new Cpf(v));  
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(u => u.Cpf)
                .HasConversion(cpfConverter)
                .HasColumnName("Cpf")
                // TODO: Add CPF Requirement.
                .HasMaxLength(11);
        });
        
        modelBuilder.Entity<User>()
            .HasMany(u => u.Addresses)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });
        
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);
        
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);

        modelBuilder.Entity<Role>().HasData(
            new Role(1, "Admin"),
            new Role(2, "Donor"),
            new Role(3, "Charity")
        );
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
}