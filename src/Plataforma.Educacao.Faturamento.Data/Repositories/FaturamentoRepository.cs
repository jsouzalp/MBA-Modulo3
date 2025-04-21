using Microsoft.EntityFrameworkCore;
using Plataforma.Educacao.Core.Data;
using Plataforma.Educacao.Core.Extensions;
using Plataforma.Educacao.Faturamento.Data.Contexts;
using Plataforma.Educacao.Faturamento.Domain.Entities;
using Plataforma.Educacao.Faturamento.Domain.Interfaces;

namespace Plataforma.Educacao.Faturamento.Data.Repositories;
public class FaturamentoRepository(FaturamentoDbContext context) : IFaturamentoRepository
{
    private readonly FaturamentoDbContext _context = context;
    public IUnitOfWork UnitOfWork => _context;

    public async Task AdicionarAsync(Pagamento pagamento)
    {
        await _context.Pagamentos.AddAsync(pagamento);
    }

    public async Task AtualizarAsync(Pagamento pagamento)
    {
        _context.Pagamentos.Update(pagamento);

        if (pagamento.Cartao != null)
        {
            _context.AtualizarEstadoValueObject(null, pagamento.Cartao);
        }

        await Task.CompletedTask;
    }

    public async Task<Pagamento> ObterPorMatriculaIdAsync(Guid matriculaId)
    {
        return await _context.Pagamentos.FirstOrDefaultAsync(p => p.MatriculaId == matriculaId);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}
