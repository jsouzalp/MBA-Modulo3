using Microsoft.EntityFrameworkCore;
using Plataforma.Educacao.Aluno.Domain.Entities;
using Plataforma.Educacao.Core.Data;
using System.Diagnostics.CodeAnalysis;

namespace Plataforma.Educacao.Aluno.Data.Contexts;

[ExcludeFromCodeCoverage]
public class AlunoDbContext : DbContext, IUnitOfWork
{
    public AlunoDbContext(DbContextOptions<AlunoDbContext> options) : base(options) { }

    public DbSet<Domain.Entities.Aluno> Alunos { get; set; }
    public DbSet<Certificado> Certificados { get; set; }
    public DbSet<MatriculaCurso> MatriculasCursos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Ignore<Event>();

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AlunoDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public async Task<bool> Commit()
    {
        return await base.SaveChangesAsync() > 0;
    }
}