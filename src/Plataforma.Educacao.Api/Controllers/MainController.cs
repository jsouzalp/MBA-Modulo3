using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Plataforma.Educacao.Api.Enumerators;
using Plataforma.Educacao.Core.Messages.Comunications;
using Plataforma.Educacao.Core.Messages.Handlers;
using Plataforma.Educacao.Core.Messages;
using System.Net;
using Plataforma.Educacao.Api.Authentications;

namespace Plataforma.Educacao.Api.Controllers;

[ApiController]
public class MainController(IAppIdentityUser appIdentityUser, 
    INotificationHandler<DomainNotificacaoRaiz> notifications, 
    IMediatorHandler mediatorHandler) : ControllerBase
{
    private readonly IAppIdentityUser _appIdentityUser = appIdentityUser;
    protected readonly DomainNotificacaoHandler _notifications = (DomainNotificacaoHandler)notifications;
    protected readonly IMediatorHandler _mediatorHandler = mediatorHandler;

    protected bool OperacaoValida() => !_notifications.TemNotificacao();
    public Guid UserId => _appIdentityUser.ObterUsuarioId();
    public bool EstahAutenticado => _appIdentityUser.EstahAutenticado();
    public string Email => _appIdentityUser.ObterEmail();
    public bool EhAdministrador => _appIdentityUser.EhAdministrador();

    protected ActionResult GenerateResponse(object? result = null,
        ResponseTypeEnum responseType = ResponseTypeEnum.Success,
        HttpStatusCode statusCode = HttpStatusCode.OK,
        IEnumerable<string> errors = null, 
        Exception exception)
    {
        if (OperacaoValida() && ((int)statusCode >= 200 && (int)statusCode <= 299))
        {
            return new JsonResult(new
            {
                success = true,
                type = responseType.ToString(),
                result
            })
            {
                StatusCode = (int)statusCode
            };
        }

        errors ??= [];
        if (_notifications.TemNotificacao())
        {
            var notificationErrors = _notifications.ObterNotificacoes().Select(n => $"Chave: {n.Chave} Mensagem: {n.Valor}").ToList();
            foreach(string erro in notificationErrors)
            {
                errors.Add(erro);
            }
        }

        return new JsonResult(new
        {
            success = false,
            type = responseType.ToString(),
            errors
        })
        {
            StatusCode = (int)statusCode
        };
    }

    protected ActionResult GenerateModelStateResponse(ResponseTypeEnum responseType, HttpStatusCode statusCode, ModelStateDictionary modelState)
    {
        return new JsonResult(new
        {
            success = false,
            type = responseType.ToString(),
            errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
        })
        {
            StatusCode = (int)statusCode
        };
    }
}