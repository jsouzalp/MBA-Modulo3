using Plataforma.Educacao.Aluno.Application.DTO;
using Plataforma.Educacao.Aluno.Application.Interfaces;
using Plataforma.Educacao.Aluno.Domain.Entities;
using Plataforma.Educacao.Aluno.Domain.Interfaces;
using Plataforma.Educacao.Aluno.Domain.ValueObjects;
using Plataforma.Educacao.Conteudo.Application.DTO;
using Plataforma.Educacao.Conteudo.Application.Interfaces;
using Plataforma.Educacao.Conteudo.Domain.Entities;
using Plataforma.Educacao.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plataforma.Educacao.Aluno.Application.Queries;
public class AlunoQueryService(IAlunoRepository alunoRepository, ICursoAppService cursoAppService) : IAlunoQueryService
{
    private readonly IAlunoRepository _alunoRepository = alunoRepository;
    private readonly ICursoAppService _cursoAppService = cursoAppService;

    public async Task<AlunoDto> ObterAlunoPorIdAsync(Guid alunoId)
    {
        var aluno = await _alunoRepository.ObterPorIdAsync(alunoId);
        if (aluno == null) return null;

        return new AlunoDto
        {
            Id = aluno.Id,
            Nome = aluno.Nome,
            Email = aluno.Email,
            DataNascimento = aluno.DataNascimento,
            MatriculasCursos = aluno.MatriculasCursos != null ? aluno.MatriculasCursos.Select(m => new MatriculaCursoDto
            {
                Id = m.Id,
                CursoId = m.CursoId,
                NomeCurso = m.NomeCurso,
                Valor = m.Valor,
                DataMatricula = m.DataMatricula,
                DataConclusao = m.DataConclusao,
                EstadoMatricula = m.EstadoMatricula.GetDescription(),
                Certificado = m.Certificado != null ? new CertificadoDto
                {
                    Id = m.Certificado.Id,
                    DataSolicitacao = m.Certificado.DataSolicitacao,
                    PathCertificado = m.Certificado.PathCertificado,
                } : null
            }).ToList() : []
        };
    }

    public async Task<EvolucaoAlunoDto> ObterEvolucaoMatriculasCursoDoAlunoPorIdAsync(Guid alunoId)
    {
        var aluno = await _alunoRepository.ObterPorIdAsync(alunoId);
        if (aluno == null) return null;

        List<CursoDto> cursos = new List<CursoDto>();
        foreach (var matricula in aluno.MatriculasCursos)
        {
            var curso = await _cursoAppService.ObterPorIdAsync(matricula.CursoId);
            if (curso != null)
            {
                cursos.Add(curso);
            }
        }

        return new EvolucaoAlunoDto
        {
            Id = aluno.Id,
            Nome = aluno.Nome,
            Email = aluno.Email,
            DataNascimento = aluno.DataNascimento,
            MatriculasCursos = aluno.MatriculasCursos != null ? aluno.MatriculasCursos.Select(m => new EvolucaoMatriculaCursoDto
            {
                Id = m.Id,
                CursoId = m.CursoId,
                NomeCurso = m.NomeCurso,
                Valor = m.Valor,
                DataMatricula = m.DataMatricula,
                DataConclusao = m.DataConclusao,
                EstadoMatricula = m.EstadoMatricula.GetDescription(),
                QuantidadeAulasNoCurso = cursos.FirstOrDefault(c => c.Id == m.CursoId)?.QuantidadeAulas ?? -1,
                QuantidadeAulasRealizadas = m.QuantidadeAulasFinalizadas,
                QuantidadeAulasEmAndamento = m.QuantidadeAulasEmAndamento,
                Certificado = m.Certificado != null ? new CertificadoDto
                {
                    Id = m.Certificado.Id,
                    DataSolicitacao = m.Certificado.DataSolicitacao,
                    PathCertificado = m.Certificado.PathCertificado,
                } : null
            }).ToList() : []
        };
    }

    public async Task<IEnumerable<MatriculaCursoDto>> ObterMatriculasPorAlunoIdAsync(Guid alunoId)
    {
        var aluno = await _alunoRepository.ObterPorIdAsync(alunoId);
        if (aluno == null) return [];

        return aluno.MatriculasCursos.Select(m => new MatriculaCursoDto
        {
            Id = m.Id,
            CursoId = m.CursoId,
            NomeCurso = m.NomeCurso,
            Valor = m.Valor,
            DataMatricula = m.DataMatricula,
            DataConclusao = m.DataConclusao,
            EstadoMatricula = m.EstadoMatricula.GetDescription(),
            Certificado = m.Certificado != null ? new CertificadoDto
            {
                Id = m.Certificado.Id,
                DataSolicitacao = m.Certificado.DataSolicitacao,
                PathCertificado = m.Certificado.PathCertificado,
            } : null
        });
    }

    public async Task<MatriculaCursoDto> ObterInformacaoMatriculaCursoParaPagamentoAsync(Guid matriculaCursoId)
    {
        var matriculaCurso = await _alunoRepository.ObterMatriculaPorIdAsync(matriculaCursoId);
        if (matriculaCurso == null) return null;

        return new MatriculaCursoDto
        {
            Id = matriculaCurso.Id,
            AlunoId = matriculaCurso.AlunoId,
            CursoId = matriculaCurso.CursoId,
            NomeCurso = matriculaCurso.NomeCurso,
            Valor = matriculaCurso.Valor,
            PagamentoPodeSerRealizado = matriculaCurso.PagamentoPodeSerRealizado,
            DataMatricula = matriculaCurso.DataMatricula,
            DataConclusao = matriculaCurso.DataConclusao,
            EstadoMatricula = matriculaCurso.EstadoMatricula.GetDescription(),
            Certificado = matriculaCurso.Certificado != null ? new CertificadoDto
            {
                Id = matriculaCurso.Certificado.Id,
                DataSolicitacao = matriculaCurso.Certificado.DataSolicitacao,
                PathCertificado = matriculaCurso.Certificado.PathCertificado,
            } : null
        };
    }
    public async Task<CertificadoDto> ObterCertificadoPorMatriculaIdAsync(Guid matriculaCursoId)
    {
        var matricula = await _alunoRepository.ObterMatriculaPorIdAsync(matriculaCursoId);
        if (matricula == null || matricula.Certificado == null) return null;

        return new CertificadoDto
        {
            Id = matricula.Certificado.Id,
            DataSolicitacao = matricula.Certificado.DataSolicitacao,
            PathCertificado = matricula.Certificado.PathCertificado
        };
    }

    public async Task<IEnumerable<AulaCursoDto>> ObterAulasPorMatriculaIdAsync(Guid matriculaCursoId)
    {
        var matricula = await _alunoRepository.ObterMatriculaPorIdAsync(matriculaCursoId);
        if (matricula == null) return null;

        var cursoDto = await _cursoAppService.ObterPorIdAsync(matricula.CursoId);
        if (cursoDto == null) return null;

        // Adiciono as aulas diretamente a partir do cursoDto
        var retorno = new List<AulaCursoDto>();
        foreach (var aula in cursoDto.Aulas)
        {
            HistoricoAprendizado historicoAprendizado = matricula.HistoricoAprendizado.FirstOrDefault(h => h.AulaId == aula.Id);

            retorno.Add(new AulaCursoDto
            {
                AulaId = aula.Id,
                CursoId = cursoDto.Id,
                NomeAula = historicoAprendizado?.NomeAula ?? aula.Descricao,
                OrdemAula = aula.OrdemAula,
                Ativo = aula.Ativo,
                DataInicio = historicoAprendizado?.DataInicio ?? null,
                DataTermino = historicoAprendizado?.DataInicio ?? null,
                Url = aula.Url
            });
        }

        // Agora adiciono as aulas a partir de MatriculaCurso que por algum motivo podem ter sido excluídas ou ficaram órfãs
        foreach (var aula in matricula.HistoricoAprendizado)
        {
            if (!retorno.Any(a => a.AulaId == aula.AulaId))
            {
                retorno.Add(new AulaCursoDto
                {
                    AulaId = aula.AulaId,
                    CursoId = aula.CursoId,
                    NomeAula = aula.NomeAula,
                    OrdemAula = 0,
                    Ativo = false,
                    DataInicio = aula.DataInicio,
                    DataTermino = aula.DataTermino,
                    Url = null
                });
            }
        }

        return retorno.OrderBy(x => x.OrdemAula).ToList();
    }
}
