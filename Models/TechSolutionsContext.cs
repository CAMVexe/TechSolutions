using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TechSolutions.Models;

public partial class TechSolutionsContext : DbContext
{
    public TechSolutionsContext()
    {
    }

    public TechSolutionsContext(DbContextOptions<TechSolutionsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Producto> Productos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=CAMVCHAR\\SQLEXPRESS;Database=TechSolutions;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProd).HasName("PK__Producto__B247ED70BB41314C");

            entity.ToTable("Producto");

            entity.Property(e => e.IdProd)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Prod");
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
