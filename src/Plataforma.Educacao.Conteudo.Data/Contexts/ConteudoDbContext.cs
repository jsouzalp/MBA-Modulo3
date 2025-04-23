using Microsoft.EntityFrameworkCore;
using Plataforma.Educacao.Conteudo.Domain.Entities;
using Plataforma.Educacao.Core.Data;
using System.Diagnostics.CodeAnalysis;

namespace Plataforma.Educacao.Conteudo.Data.Contexts;

[ExcludeFromCodeCoverage]
public class ConteudoDbContext : DbContext, IUnitOfWork
{
    public ConteudoDbContext(DbContextOptions<ConteudoDbContext> options) : base(options) { }

    public DbSet<Aula> Aulas { get; set; }
    public DbSet<Curso> Cursos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Ignore<Event>();

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ConteudoDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public async Task<bool> Commit()
    {
        return await base.SaveChangesAsync() > 0;
    }
}