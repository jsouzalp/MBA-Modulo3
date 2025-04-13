using Plataforma.Educacao.Aluno.Domain.Entities;
using Plataforma.Educacao.Core.Data;

namespace Plataforma.Educacao.Aluno.Domain.Interfaces;
public interface IAlunoRepository : IRepository<Entities.Aluno>
{
    #region Aluno
    Task<Entities.Aluno> ObterPorIdAsync(Guid alunoId);
    Task<Entities.Aluno> ObterPorEmailAsync(string email);
    Task<bool> ExisteEmailAsync(string email);
    Task AdicionarAsync(Entities.Aluno aluno);
    Task AtualizarAsync(Entities.Aluno aluno);
    #endregion

    #region Matrícula
    Task<MatriculaCurso> ObterMatriculaPorIdAsync(Guid matriculaId);
    Task<MatriculaCurso> ObterMatriculaPorAlunoECursoAsync(Guid alunoId, Guid cursoId);
    #endregion

    #region Certificado
    Task<Certificado> ObterCertificadoPorMatriculaAsync(Guid matriculaId);
    #endregion
}
