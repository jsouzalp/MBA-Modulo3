using Plataforma.Educacao.Conteudo.Data.Contexts;
using Plataforma.Educacao.Conteudo.Domain.Interfaces;
using Plataforma.Educacao.Core.Data;

namespace Plataforma.Educacao.Conteudo.Data.Repositories
{
    public class CursoRepository : ICursoRepository
    {
        private readonly ConteudoDbContext _context;
        //public IUnitOfWork UnitOfWork => _context;

        public CursoRepository(ConteudoDbContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
