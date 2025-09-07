using Microsoft.EntityFrameworkCore;
using StajyerTakip.Domain.Entities;

namespace StajyerTakip.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Intern> Interns => Set<Intern>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Intern>(e =>
        {
            e.HasIndex(x => x.Email).IsUnique();
            e.HasIndex(x => x.NationalId).IsUnique();
        });
    }
}
