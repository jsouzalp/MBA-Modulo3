using Plataforma.Educacao.Core.Data;
using Plataforma.Educacao.Faturamento.Data.Contexts;
using Plataforma.Educacao.Faturamento.Domain.Interfaces;

namespace Plataforma.Educacao.Faturamento.Data.Repositories
{
    public class PagamentoRepository : IPagamentoCommandRepository
    {
        private readonly PagamentoDbContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public PagamentoRepository(PagamentoDbContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
