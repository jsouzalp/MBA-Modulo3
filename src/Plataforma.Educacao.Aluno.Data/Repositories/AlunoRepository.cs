using Plataforma.Educacao.Aluno.Data.Contexts;
using Plataforma.Educacao.Aluno.Domain.Interfaces;
using Plataforma.Educacao.Core.Data;

namespace Plataforma.Educacao.Aluno.Data.Repositories
{
    public class AlunoRepository : IAlunoRepository
    {
        private readonly AlunoDbContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public AlunoRepository(AlunoDbContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
