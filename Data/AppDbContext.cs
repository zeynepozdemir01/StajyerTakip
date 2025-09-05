using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using StajyerTakip.Models;

namespace StajyerTakip.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Intern> Interns { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Intern>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Interns__3214EC07AA7C56BE");

            entity.ToTable(tb => tb.HasTrigger("trg_Interns_SetUpdatedAt"));

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Status).HasDefaultValue("Aktif");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
