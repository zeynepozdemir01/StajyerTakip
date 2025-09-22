using Microsoft.EntityFrameworkCore;
using StajyerTakip.Domain.Entities;

namespace StajyerTakip.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Intern> Interns => Set<Intern>();

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);

        b.Entity<RefreshToken>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Username).HasMaxLength(100).IsRequired();
            e.Property(x => x.Token).HasMaxLength(200).IsRequired();
            e.Property(x => x.ExpiresAt).IsRequired();
            e.HasIndex(x => x.Token).IsUnique();
        });

        b.Entity<Intern>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
            e.Property(x => x.LastName).IsRequired().HasMaxLength(100);
            e.Property(x => x.Email).IsRequired().HasMaxLength(200);
            e.Property(x => x.NationalId).IsRequired().HasMaxLength(11);

            e.HasIndex(x => x.Email).IsUnique();
            e.HasIndex(x => x.NationalId).IsUnique();
        });
    }
}
