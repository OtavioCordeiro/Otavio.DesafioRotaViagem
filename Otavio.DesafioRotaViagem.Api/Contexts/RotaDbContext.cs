using Microsoft.EntityFrameworkCore;
using Otavio.DesafioRotaViagem.Api.Entities;

namespace Otavio.DesafioRotaViagem.Api.Contexts
{
    public class RotaDbContext : DbContext
    {
        public RotaDbContext(DbContextOptions<RotaDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Rota> Rotas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Rota>().HasData(
                new Rota { Id = 1, Origem = "GRU", Destino = "BRC", Valor = 10 },
                new Rota { Id = 2, Origem = "BRC", Destino = "SCL", Valor = 5 },
                new Rota { Id = 3, Origem = "GRU", Destino = "CDG", Valor = 75 },
                new Rota { Id = 4, Origem = "GRU", Destino = "SCL", Valor = 20 },
                new Rota { Id = 5, Origem = "GRU", Destino = "ORL", Valor = 56 },
                new Rota { Id = 6, Origem = "ORL", Destino = "CDG", Valor = 5 },
                new Rota { Id = 7, Origem = "SCL", Destino = "ORL", Valor = 20 }
            );
        }
    }
}
