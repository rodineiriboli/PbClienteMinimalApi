using ClienteApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ClienteApi.Data
{
    public class ClienteContextDb : DbContext
    {
        public ClienteContextDb(DbContextOptions<ClienteContextDb> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Cliente>()
                .Property(x => x.Nome)
                .IsRequired()
                .HasColumnType("varchar(250)");

            modelBuilder.Entity<Cliente>()
                .Property(x => x.Email)
                .IsRequired()
                .HasColumnType("varchar(200)");

            modelBuilder.Entity<Cliente>()
                .ToTable("Clientes");

            base.OnModelCreating(modelBuilder);
        }
    }
}
