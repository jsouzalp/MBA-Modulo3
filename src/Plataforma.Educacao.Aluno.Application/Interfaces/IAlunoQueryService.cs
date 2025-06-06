using Plataforma.Educacao.Aluno.Application.DTO;
using Plataforma.Educacao.Core.SharedDto.Aluno;
using Plataforma.Educacao.Core.SharedDto.Conteudo;

namespace Plataforma.Educacao.Aluno.Application.Interfaces;
public interface IAlunoQueryService 
{
    Task<AlunoDto> ObterAlunoPorIdAsync(Guid alunoId);
    Task<EvolucaoAlunoDto> ObterEvolucaoMatriculasCursoDoAlunoPorIdAsync(Guid alunoId);
    Task<IEnumerable<MatriculaCursoDto>> ObterMatriculasPorAlunoIdAsync(Guid alunoId);
    Task<MatriculaCursoDto> ObterInformacaoMatriculaCursoAsync(Guid matriculaCursoId);
    Task<CertificadoDto> ObterCertificadoPorMatriculaIdAsync(Guid matriculaCursoId);
    Task<IEnumerable<AulaCursoDto>> ObterAulasPorMatriculaIdAsync(Guid matriculaCursoId, CursoDto cursoDto);
}
