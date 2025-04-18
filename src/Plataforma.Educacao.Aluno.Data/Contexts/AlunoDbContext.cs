using Microsoft.EntityFrameworkCore;
using Plataforma.Educacao.Aluno.Domain.Entities;
using Plataforma.Educacao.Core.Data;

namespace Plataforma.Educacao.Aluno.Data.Contexts;
public class AlunoDbContext : DbContext, IUnitOfWork
{
    public AlunoDbContext(DbContextOptions<AlunoDbContext> options) : base(options) { }

    public DbSet<Domain.Entities.Aluno> Alunos { get; set; }
    public DbSet<Certificado> Certificados { get; set; }
    public DbSet<MatriculaCurso> MatriculasCursos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
        //    e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
        //    property.Relational().ColumnType = "varchar(100)";

        //modelBuilder.Ignore<Event>();

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AlunoDbContext).Assembly);
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