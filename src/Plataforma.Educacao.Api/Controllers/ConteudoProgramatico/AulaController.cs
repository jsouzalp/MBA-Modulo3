using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plataforma.Educacao.Api.Enumerators;
using Plataforma.Educacao.Conteudo.Application.DTO;
using Plataforma.Educacao.Conteudo.Application.Interfaces;
using Plataforma.Educacao.Core.Exceptions;
using Plataforma.Educacao.Core.Messages.Comunications;
using Plataforma.Educacao.Core.Messages;
using System.Net;
using Plataforma.Educacao.Api.ViewModels.ConteudoProgramatico;
using AutoMapper;

namespace Plataforma.Educacao.Api.Controllers.ConteudoProgramatico;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AulaController(IAulaAppService aulaAppService,
    IMapper mapper,
    INotificationHandler<DomainNotificacaoRaiz> notifications,
    IMediatorHandler mediatorHandler) : MainController(notifications, mediatorHandler)
{
    private readonly IAulaAppService _aulaAppService = aulaAppService;
    private readonly IMapper _mapper = mapper;

    [HttpPost("{cursoId:guid}")]
    public async Task<IActionResult> AdicionarAula(Guid cursoId, [FromBody] AulaViewModel aulaViewModel)
    {
        if (!ModelState.IsValid) { return GenerateModelStateResponse(ResponseTypeEnum.ValidationError, HttpStatusCode.BadRequest, ModelState); }

        try
        {
            var dto = _mapper.Map<AulaDto>(aulaViewModel);
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
    public async Task<IActionResult> AtualizarAula(Guid cursoId, [FromBody] AulaViewModel aulaViewModel)
    {
        if (!ModelState.IsValid) { return GenerateModelStateResponse(ResponseTypeEnum.ValidationError, HttpStatusCode.BadRequest, ModelState); }

        try
        {
            var dto = _mapper.Map<AulaDto>(aulaViewModel);
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