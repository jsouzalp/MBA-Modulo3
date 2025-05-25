using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plataforma.Educacao.Api.Authentications;
using Plataforma.Educacao.Core.Messages.Comunications;
using Plataforma.Educacao.Core.Messages;
using Plataforma.Educacao.Api.Enumerators;
using Plataforma.Educacao.Core.Exceptions;
using System.Net;
using Plataforma.Educacao.Api.ViewModels.Faturamento.Commands;
using Plataforma.Educacao.Core.Messages.Comunications.FaturamentoCommands;
using Plataforma.Educacao.Aluno.Application.Interfaces;

namespace Plataforma.Educacao.Api.Controllers.Faturamento;

[Authorize(Policy = "ApenasAluno")]
[ApiController]
[Route("api/[controller]")]
public class FaturamentoController(IAppIdentityUser appIdentityUser,
    IAlunoQueryService alunoQueryService,
    INotificationHandler<DomainNotificacaoRaiz> notifications,
    IMediatorHandler mediatorHandler) : MainController(appIdentityUser, notifications, mediatorHandler)
{
    private readonly IAlunoQueryService _alunoQueryService = alunoQueryService;

    [HttpPost("{alunoId}/registrar-pagamento")]
    public async Task<IActionResult> RealizarPagamento(Guid alunoId, RealizarPagamentoViewModel pagamentoViewModel)
    {
        if (!ModelState.IsValid) { return GenerateModelStateResponse(ResponseTypeEnum.ValidationError, HttpStatusCode.BadRequest, ModelState); }

        try
        {
            if (UserId != pagamentoViewModel.AlunoId) { return GenerateResponse(null, ResponseTypeEnum.ValidationError, HttpStatusCode.Forbidden, ["Você não tem permissão para realizar essa operação"]); }
            var matriculaCursoDto = await _alunoQueryService.ObterInformacaoMatriculaCursoAsync(pagamentoViewModel.MatriculaCursoId);
            if (matriculaCursoDto == null) { return GenerateResponse(null, ResponseTypeEnum.ValidationError, HttpStatusCode.NotFound, ["Matrícula do curso não encontrada"]); }

            var comando = new RealizarPagamentoCommand(pagamentoViewModel.MatriculaCursoId, matriculaCursoDto, pagamentoViewModel.Valor, pagamentoViewModel.NumeroCartao, pagamentoViewModel.NomeTitularCartao, pagamentoViewModel.ValidadeCartao, pagamentoViewModel.CvvCartao);
            var sucesso = await _mediatorHandler.EnviarComando(comando);
            if (sucesso)
            {
                return GenerateResponse(new { pagamentoViewModel.AlunoId, pagamentoViewModel.MatriculaCursoId },
                    responseType: ResponseTypeEnum.Success,
                    statusCode: HttpStatusCode.Created);
            }

            return GenerateResponse(responseType: ResponseTypeEnum.GenericError, statusCode: HttpStatusCode.BadRequest);
        }
        catch (DomainException exDomain)
        {
            return GenerateDomainExceptionResponse(null, ResponseTypeEnum.DomainError, HttpStatusCode.BadRequest, exDomain);
        }
        catch (Exception ex)
        {
            return GenerateResponse(null, ResponseTypeEnum.GenericError, HttpStatusCode.BadRequest, [ex.Message]);
        }
    }
}
