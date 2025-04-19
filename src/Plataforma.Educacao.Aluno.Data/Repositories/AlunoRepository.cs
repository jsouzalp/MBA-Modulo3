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

    #region Alunos
    public async Task AdicionarAsync(Domain.Entities.Aluno aluno)
    {
        await _context.Alunos.AddAsync(aluno);
    }

    public async Task AtualizarAsync(Domain.Entities.Aluno aluno)
    {
        _context.Alunos.Update(aluno);
        await Task.CompletedTask;
    }

    public async Task<Domain.Entities.Aluno> ObterPorIdAsync(Guid alunoId)
    {
        return await _context.Alunos
            .Include(a => a.MatriculasCursos)
            .ThenInclude(m => m.Certificado)
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == alunoId);
    }

    public async Task<Domain.Entities.Aluno> ObterPorEmailAsync(string email)
    {
        return await _context.Alunos
            .Include(a => a.MatriculasCursos)
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Email == email);
    }

    public async Task<bool> ExisteEmailAsync(string email)
    {
        return await _context.Alunos.AnyAsync(a => a.Email == email);
    }
    #endregion

    #region Matricula Curso
    public async Task AdicionarMatriculaCursoAsync(MatriculaCurso matriculaCurso)
    {
        await _context.MatriculasCursos.AddAsync(matriculaCurso);
    }

    public async Task<MatriculaCurso> ObterMatriculaPorIdAsync(Guid matriculaId)
    {
        return await _context.MatriculasCursos
            .Include(m => m.HistoricoAprendizado)
            .Include(m => m.Certificado)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == matriculaId);
    }

    public async Task<MatriculaCurso> ObterMatriculaPorAlunoECursoAsync(Guid alunoId, Guid cursoId)
    {
        return await _context.MatriculasCursos
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.AlunoId == alunoId && m.CursoId == cursoId);
    }
    #endregion

    #region Certificado
    public async Task<Certificado> ObterCertificadoPorMatriculaAsync(Guid matriculaId)
    {
        return await _context.Certificados
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.MatriculaCursoId == matriculaId);
    }
    #endregion

    public void Dispose()
    {
        _context?.Dispose();
        GC.SuppressFinalize(this);
    }
}