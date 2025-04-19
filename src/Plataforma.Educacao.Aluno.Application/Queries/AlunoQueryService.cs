using Plataforma.Educacao.Aluno.Application.DTO;
using Plataforma.Educacao.Aluno.Application.Interfaces;
using Plataforma.Educacao.Aluno.Domain.Entities;
using Plataforma.Educacao.Aluno.Domain.Interfaces;
using Plataforma.Educacao.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plataforma.Educacao.Aluno.Application.Queries;
public class AlunoQueryService(IAlunoRepository alunoRepository) : IAlunoQueryService
{
    private readonly IAlunoRepository _alunoRepository = alunoRepository;

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
}
