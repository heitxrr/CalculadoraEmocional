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

        public DbSet<Checkin> Checkins { get; set; }
        public DbSet<IndiceEmocional> IndicesEmocionais { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Checkin>().ToTable("Checkins");
            modelBuilder.Entity<IndiceEmocional>().ToTable("IndicesEmocionais");
        }
    }
}
