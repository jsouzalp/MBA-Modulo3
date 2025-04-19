using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Plataforma.Educacao.Api.Enumerators;
using Plataforma.Educacao.Api.ViewModels.Aluno.Queries;
using System.Net;

namespace Plataforma.Educacao.Api.Controllers.Aluno;

public partial class AlunoController
{
    [HttpGet("{id}")]
    public async Task<IActionResult> ObterAlunoPorId(Guid id)
    {
        var aluno = await _alunoQueryService.ObterAlunoPorIdAsync(id);
        if (aluno == null) { return GenerateResponse(null, ResponseTypeEnum.NotFound, HttpStatusCode.NotFound); }

        return GenerateResponse(_mapper.Map<AlunoViewModel>(aluno));
    }

    [HttpGet("/{matriculaId}/matriculas")]
    public async Task<IActionResult> ObterMatriculasPorAlunoId(Guid matriculaId)
    {
        var matriculas = await _alunoQueryService.ObterMatriculasPorAlunoIdAsync(matriculaId);
        if (matriculas != null || !matriculas.Any()) { return GenerateResponse(null, ResponseTypeEnum.NotFound, HttpStatusCode.NotFound); }

        return GenerateResponse(_mapper.Map<MatriculaCursoViewModel>(matriculas));
    }

    [HttpGet("/{matriculaId}/matricula/certificado")]
    public async Task<IActionResult> ObterCertificadoPorMatriculaId(Guid matriculaId)
    {
        var certificado = await _alunoQueryService.ObterCertificadoPorMatriculaIdAsync(matriculaId);
        if (certificado == null) { return GenerateResponse(null, ResponseTypeEnum.NotFound, HttpStatusCode.NotFound); }

        return GenerateResponse(_mapper.Map<CertificadoViewModel>(certificado));
    }
}
