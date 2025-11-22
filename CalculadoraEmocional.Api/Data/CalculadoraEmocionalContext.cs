using CalculadoraEmocional.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace CalculadoraEmocional.Api.Data
{
    public class CalculadoraEmocionalContext : DbContext
    {
        public CalculadoraEmocionalContext(DbContextOptions<CalculadoraEmocionalContext> options)
            : base(options)
        {
        }

        public DbSet<Checkin> Checkins { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Checkin>(entity =>
            {
                entity.ToTable("T_GS_CHECKIN");

                entity.HasKey(e => e.IdCheckin);

                entity.Property(e => e.IdCheckin).HasColumnName("id_checkin");
                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
                entity.Property(e => e.DataCheckin).HasColumnName("dt_checkin");
                entity.Property(e => e.Humor).HasColumnName("vl_humor");
                entity.Property(e => e.Foco).HasColumnName("vl_foco");
                entity.Property(e => e.MinutosPausas).HasColumnName("minutos_pausas");
                entity.Property(e => e.HorasTrabalhadas)
                      .HasColumnName("horas_trabalhadas")
                      .HasColumnType("decimal(5,2)");
                entity.Property(e => e.Observacoes).HasColumnName("ds_observacoes");
                entity.Property(e => e.Tags).HasColumnName("tags").HasMaxLength(200);
                entity.Property(e => e.Origem).HasColumnName("origem").HasMaxLength(20);
            });
        }
    }
}
