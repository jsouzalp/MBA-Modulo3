using Microsoft.EntityFrameworkCore;
using Plataforma.Educacao.Core.Data;
using Plataforma.Educacao.Faturamento.Domain.Entities;

namespace Plataforma.Educacao.Faturamento.Data.Contexts
{
    public class PagamentoDbContext : DbContext, IUnitOfWork
    {
        public PagamentoDbContext(DbContextOptions<PagamentoDbContext> options) : base(options) { }

        public DbSet<Pagamento> Pagamentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
            //    e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
            //    property.Relational().ColumnType = "varchar(100)";

            //modelBuilder.Ignore<Event>();

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PagamentoDbContext).Assembly);
        }

        public async Task<bool> Commit()
        {
            //foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") != null))
            //{
            //    if (entry.State == EntityState.Added)
            //    {
            //        entry.Property("DataCadastro").CurrentValue = DateTime.Now;
            //    }

            //    if (entry.State == EntityState.Modified)
            //    {
            //        entry.Property("DataCadastro").IsModified = false;
            //    }
            //}

            return await base.SaveChangesAsync() > 0;
        }
    }
}