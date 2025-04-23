using Microsoft.EntityFrameworkCore;
using Plataforma.Educacao.Core.Data;
using Plataforma.Educacao.Faturamento.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Plataforma.Educacao.Faturamento.Data.Contexts;

[ExcludeFromCodeCoverage]
public class FaturamentoDbContext(DbContextOptions<FaturamentoDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Pagamento> Pagamentos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Ignore<Event>();

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FaturamentoDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public async Task<bool> Commit()
    {
        return await base.SaveChangesAsync() > 0;
    }
}