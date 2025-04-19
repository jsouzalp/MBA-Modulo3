using Plataforma.Educacao.Aluno.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plataforma.Educacao.Aluno.Application.Interfaces;
public interface IAlunoQueryService
{
    Task<AlunoDto> ObterAlunoPorIdAsync(Guid alunoId);
    Task<IEnumerable<MatriculaCursoDto>> ObterMatriculasPorAlunoIdAsync(Guid alunoId);
    Task<CertificadoDto> ObterCertificadoPorMatriculaIdAsync(Guid matriculaCursoId);
}
