using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace TokaAPI.Models
{
    public partial class TokaContext : DbContext
    {
        public TokaContext()
        {
        }

        public TokaContext(DbContextOptions<TokaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TbPersonasFisica> TbPersonasFisicas { get; set; }
        public virtual DbSet<TbUsuarios> TbUsuarios { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<TbPersonasFisica>(entity =>
            {
                entity.HasKey(e => e.IdPersonaFisica);
                
                entity.Property(e => e.Activo).HasDefaultValueSql("((1))");

                entity.Property(e => e.ApellidoMaterno).IsUnicode(false);

                entity.Property(e => e.ApellidoPaterno).IsUnicode(false);

                entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Nombre).IsUnicode(false);

                entity.Property(e => e.Rfc).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
