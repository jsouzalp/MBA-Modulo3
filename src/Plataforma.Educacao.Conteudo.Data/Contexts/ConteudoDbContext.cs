using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Plataforma.Educacao.Conteudo.Domain.Entities;
using Plataforma.Educacao.Core.Data;

namespace Plataforma.Educacao.Conteudo.Data.Contexts
{
    public class ConteudoDbContext : DbContext, IUnitOfWork
    {
        public ConteudoDbContext(DbContextOptions<ConteudoDbContext> options) : base(options) { }

        public DbSet<Aula> Aulas { get; set; }
        public DbSet<Curso> Cursos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
            //    e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
            //    property.Relational().ColumnType = "varchar(100)";

            //modelBuilder.Ignore<Event>();

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ConteudoDbContext).Assembly);
            //foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            base.OnModelCreating(modelBuilder);
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