using DoeMais.Data.Interceptors;
using DoeMais.Domain;
using Microsoft.EntityFrameworkCore;
using DoeMais.Domain.Entities;
using DoeMais.Domain.ValueObjects;
using DoeMais.Services.Query;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DoeMais.Data;

public class AppDbContext : DbContext
{
    private readonly ICurrentUserService _currentUserService;
    private readonly UserIdSaveChangesInterceptor _userIdInterceptor;
    
    public AppDbContext()
    {
        
    }
    public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUserService currentUserService)
        : base(options)
    {
        _currentUserService = currentUserService;
        _userIdInterceptor = new UserIdSaveChangesInterceptor(_currentUserService);
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_userIdInterceptor);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            modelBuilder.Entity(entityType.ClrType)
                .Property<DateTime>("CreatedAt")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity(entityType.ClrType)
                .Property<DateTime?>("UpdatedAt");
        }
        
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
                .HasMaxLength(11);
        });
        
        modelBuilder.Entity<Address>().HasQueryFilter(a => a.UserId == _currentUserService.UserId);
        modelBuilder.Entity<Donation>().HasQueryFilter(d => d.UserId == _currentUserService.UserId);
        
        modelBuilder.Entity<User>()
            .HasMany(u => u.Addresses)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<User>()
            .HasMany(u => u.Donations)
            .WithOne(d => d.User)
            .HasForeignKey(u => u.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Ignore<AddressSnapshot>();
        modelBuilder.Entity<Donation>(e =>
        {
            e.OwnsOne(d => d.AddressSnapshot, a =>
            {
                a.WithOwner();
                a.Property(p => p.Street).HasMaxLength(160).IsRequired();
                a.Property(p => p.Complement).HasMaxLength(80);
                a.Property(p => p.Neighborhood).HasMaxLength(80);
                a.Property(p => p.City).HasMaxLength(80).IsRequired();
                a.Property(p => p.State).HasMaxLength(2).IsRequired();
                a.Property(p => p.ZipCode).HasMaxLength(12).IsRequired();
            });
        });
        
        modelBuilder.Entity<Donation>()
            .Property(d => d.Category)
            .HasConversion<string>();
        
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

        var seedDate = new DateTime(2025, 07, 30, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<Role>().HasData(
            new { RoleId = 1L, Name = "Admin", CreatedAt = seedDate, UpdatedAt = seedDate },
            new { RoleId = 2L, Name = "Donor", CreatedAt = seedDate, UpdatedAt = seedDate },
            new { RoleId = 3L, Name = "Charity", CreatedAt = seedDate, UpdatedAt = seedDate }
        );
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Added or EntityState.Modified && e.Metadata.IsOwned() == false);

        foreach (var entry in entries)
        {
            entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;

            if (entry.State == EntityState.Added)
            {
                entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
            }
        }
    }
    
    
    // I've set these to virtual because of the tests. Mocks need these
    // properties to be set as `virtual`, there is no lazy loading here.
    public virtual DbSet<User> Users => Set<User>();
    public virtual DbSet<Address> Addresses => Set<Address>();
    public virtual DbSet<Role> Roles => Set<Role>();
    public virtual DbSet<UserRole> UserRoles => Set<UserRole>();
    public virtual DbSet<Donation> Donations => Set<Donation>();
}