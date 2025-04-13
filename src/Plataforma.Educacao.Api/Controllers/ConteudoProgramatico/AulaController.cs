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
public class AulaController(IAulaAppService aulaAppService) : MainController
{
    private readonly IAulaAppService _aulaAppService = aulaAppService;

    [HttpPost("{cursoId:guid}")]
    public async Task<IActionResult> AdicionarAula(Guid cursoId, [FromBody] AulaDto dto)
    {
        if (!ModelState.IsValid) { return GenerateModelStateResponse(ResponseTypeEnum.ValidationError, HttpStatusCode.BadRequest, ModelState); }

        try
        {
            var aulaId = await _aulaAppService.AdicionarAulaAsync(cursoId, dto);
            return GenerateResponse(new { AulaId = aulaId }, ResponseTypeEnum.Success, HttpStatusCode.Created);
        }
        catch (DomainException exDomain)
        {
            return GenerateResponse(null, ResponseTypeEnum.DomainError, HttpStatusCode.BadRequest, [exDomain.Message]);
        }
        catch (Exception ex)
        {
            return GenerateResponse(null, ResponseTypeEnum.GenericError, HttpStatusCode.BadRequest, [ex.Message]);
        }
    }

    [HttpPut("{cursoId:guid}")]
    public async Task<IActionResult> AtualizarAula(Guid cursoId, [FromBody] AulaDto dto)
    {
        if (!ModelState.IsValid) { return GenerateModelStateResponse(ResponseTypeEnum.ValidationError, HttpStatusCode.BadRequest, ModelState); }

        try
        {
            await _aulaAppService.AtualizarAulaAsync(cursoId, dto);
            return GenerateResponse(null, ResponseTypeEnum.Success, HttpStatusCode.NoContent);
        }
        catch (DomainException exDomain)
        {
            return GenerateResponse(null, ResponseTypeEnum.DomainError, HttpStatusCode.BadRequest, [exDomain.Message]);
        }
        catch (Exception ex)
        {
            return GenerateResponse(null, ResponseTypeEnum.GenericError, HttpStatusCode.BadRequest, [ex.Message]);
        }

    }

    [HttpDelete("{cursoId:guid}/{aulaId:guid}")]
    public async Task<IActionResult> RemoverAula(Guid cursoId, Guid aulaId)
    {
        try
        {
            await _aulaAppService.RemoverAulaAsync(cursoId, aulaId);
            return GenerateResponse(null, ResponseTypeEnum.Success, HttpStatusCode.NoContent);
        }
        catch (DomainException exDomain)
        {
            return GenerateResponse(null, ResponseTypeEnum.DomainError, HttpStatusCode.BadRequest, [exDomain.Message]);
        }
        catch (Exception ex)
        {
            return GenerateResponse(null, ResponseTypeEnum.GenericError, HttpStatusCode.BadRequest, [ex.Message]);
        }
    }
}