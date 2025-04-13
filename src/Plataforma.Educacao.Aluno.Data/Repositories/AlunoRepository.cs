using Microsoft.EntityFrameworkCore;
using Plataforma.Educacao.Aluno.Data.Contexts;
using Plataforma.Educacao.Aluno.Domain.Entities;
using Plataforma.Educacao.Aluno.Domain.Interfaces;
using Plataforma.Educacao.Core.Data;

namespace Plataforma.Educacao.Aluno.Data.Repositories;
public class AlunoRepository(AlunoDbContext context) : IAlunoRepository
{
    private readonly AlunoDbContext _context = context;
    public IUnitOfWork UnitOfWork => _context;

    #region Aluno
    public async Task<Domain.Entities.Aluno> ObterPorIdAsync(Guid alunoId)
    {
        return await _context.Alunos
            .Include(a => a.MatriculasCursos)
                .ThenInclude(m => m.Certificado)
            .FirstOrDefaultAsync(a => a.Id == alunoId);
    }

    public async Task<Domain.Entities.Aluno> ObterPorEmailAsync(string email)
    {
        return await _context.Alunos
            .Include(a => a.MatriculasCursos)
                .ThenInclude(m => m.Certificado)
            .FirstOrDefaultAsync(a => a.Email == email);
    }

    public async Task<bool> ExisteEmailAsync(string email)
    {
        return await _context.Alunos.AnyAsync(a => a.Email == email);
    }

    public async Task AdicionarAsync(Domain.Entities.Aluno aluno)
    {
        await _context.Alunos.AddAsync(aluno);
    }

    public Task AtualizarAsync(Domain.Entities.Aluno aluno)
    {
        _context.Alunos.Update(aluno);
        return Task.CompletedTask;
    }
    #endregion

    #region Matrícula
    public async Task<MatriculaCurso> ObterMatriculaPorIdAsync(Guid matriculaId)
    {
        return await _context.MatriculaCursos
            .Include(m => m.Certificado)
            .Include(m => m.HistoricoAprendizado)
            .FirstOrDefaultAsync(m => m.Id == matriculaId);
    }

    public async Task<MatriculaCurso> ObterMatriculaPorAlunoECursoAsync(Guid alunoId, Guid cursoId)
    {
        return await _context.MatriculaCursos
            .Include(m => m.Certificado)
            .Include(m => m.HistoricoAprendizado)
            .FirstOrDefaultAsync(m => m.AlunoId == alunoId && m.CursoId == cursoId);
    }
    #endregion

    #region Certificado
    public async Task<Certificado> ObterCertificadoPorMatriculaAsync(Guid matriculaId)
    {
        return await _context.Certificados
            .FirstOrDefaultAsync(c => c.MatriculaCursoId == matriculaId);
    }
    #endregion

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
