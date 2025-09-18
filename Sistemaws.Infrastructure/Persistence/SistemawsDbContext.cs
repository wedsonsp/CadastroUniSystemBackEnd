using Microsoft.EntityFrameworkCore;
using Sistemaws.Domain.Entities;

namespace Sistemaws.Infrastructure.Persistence;

public class SistemawsDbContext : DbContext
{
    public SistemawsDbContext(DbContextOptions<SistemawsDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // BaseEntity configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.Salt).IsRequired();
            entity.Property(e => e.IsActive).IsRequired();
            entity.Property(e => e.Deleted).IsRequired().HasDefaultValue(false);
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt);

            entity.HasIndex(e => e.Email).IsUnique();
        });

        base.OnModelCreating(modelBuilder);
    }
}
