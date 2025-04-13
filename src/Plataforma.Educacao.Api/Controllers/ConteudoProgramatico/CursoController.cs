using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plataforma.Educacao.Api.Enumerators;
using Plataforma.Educacao.Conteudo.Application.DTO;
using Plataforma.Educacao.Conteudo.Application.Interfaces;
using Plataforma.Educacao.Core.Exceptions;
using System.Net;

namespace Plataforma.Educacao.Api.Controllers.ConteudoProgramatico;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CursoController(ICursoAppService cursoAppService) : MainController
{
    private readonly ICursoAppService _cursoAppService = cursoAppService;

    [HttpPost]
    public async Task<IActionResult> CadastrarCurso([FromBody] CadastroCursoDto dto)
    {
        if (!ModelState.IsValid)
            return GenerateModelStateResponse(ResponseTypeEnum.ValidationError, HttpStatusCode.BadRequest, ModelState);

        try
        {
            var cursoId = await _cursoAppService.CadastrarCursoAsync(dto);
            return GenerateResponse(new { CursoId = cursoId }, ResponseTypeEnum.Success, HttpStatusCode.Created);
        }
        catch (DomainException exDomain)
        {
            return GenerateResponse(null, ResponseTypeEnum.DomainError, HttpStatusCode.BadRequest, [exDomain.Message]);
        }
        catch (Exception ex)
        {
            return GenerateResponse(null, ResponseTypeEnum.GenericError, HttpStatusCode.InternalServerError, [ex.Message]);
        }
    }

    [HttpPut("{cursoId:guid}")]
    public async Task<IActionResult> AtualizarCurso(Guid cursoId, [FromBody] AtualizacaoCursoDto dto)
    {
        if (!ModelState.IsValid)
            return GenerateModelStateResponse(ResponseTypeEnum.ValidationError, HttpStatusCode.BadRequest, ModelState);

        try
        {
            await _cursoAppService.AtualizarCursoAsync(cursoId, dto);
            return GenerateResponse(null, ResponseTypeEnum.Success, HttpStatusCode.NoContent);
        }
        catch (DomainException exDomain)
        {
            return GenerateResponse(null, ResponseTypeEnum.DomainError, HttpStatusCode.BadRequest, [exDomain.Message]);
        }
        catch (Exception ex)
        {
            return GenerateResponse(null, ResponseTypeEnum.GenericError, HttpStatusCode.InternalServerError, [ex.Message]);
        }
    }

    [HttpPatch("{cursoId:guid}/desativar")]
    public async Task<IActionResult> DesativarCurso(Guid cursoId)
    {
        try
        {
            await _cursoAppService.DesativarCursoAsync(cursoId);
            return GenerateResponse(null, ResponseTypeEnum.Success, HttpStatusCode.NoContent);
        }
        catch (DomainException exDomain)
        {
            return GenerateResponse(null, ResponseTypeEnum.DomainError, HttpStatusCode.BadRequest, [exDomain.Message]);
        }
        catch (Exception ex)
        {
            return GenerateResponse(null, ResponseTypeEnum.GenericError, HttpStatusCode.InternalServerError, [ex.Message]);
        }
    }

    [HttpGet("{cursoId:guid}")]
    public async Task<IActionResult> ObterPorId(Guid cursoId)
    {
        try
        {
            var curso = await _cursoAppService.ObterPorIdAsync(cursoId);
            return GenerateResponse(curso, ResponseTypeEnum.Success, HttpStatusCode.OK);
        }
        catch (DomainException exDomain)
        {
            return GenerateResponse(null, ResponseTypeEnum.DomainError, HttpStatusCode.NotFound, [exDomain.Message]);
        }
        catch (Exception ex)
        {
            return GenerateResponse(null, ResponseTypeEnum.GenericError, HttpStatusCode.InternalServerError, [ex.Message]);
        }
    }

    [HttpGet("ativos")]
    public async Task<IActionResult> ObterAtivos()
    {
        try
        {
            var cursos = await _cursoAppService.ObterAtivosAsync();
            return GenerateResponse(cursos, ResponseTypeEnum.Success, HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return GenerateResponse(null, ResponseTypeEnum.GenericError, HttpStatusCode.InternalServerError, [ex.Message]);
        }
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodos()
    {
        try
        {
            var cursos = await _cursoAppService.ObterTodosAsync();
            return GenerateResponse(cursos, ResponseTypeEnum.Success, HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return GenerateResponse(null, ResponseTypeEnum.GenericError, HttpStatusCode.InternalServerError, [ex.Message]);
        }
    }
}