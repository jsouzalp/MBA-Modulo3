using Plataforma.Educacao.Core.Data;
using Plataforma.Educacao.Faturamento.Data.Contexts;
using Plataforma.Educacao.Faturamento.Domain.Interfaces;

namespace Plataforma.Educacao.Faturamento.Data.Repositories
{
    public class FaturamentoRepository : IFaturamentoRepository
    {
        private readonly FaturamentoDbContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public FaturamentoRepository(FaturamentoDbContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
