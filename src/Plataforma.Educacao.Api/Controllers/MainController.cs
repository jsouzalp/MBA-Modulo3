using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Plataforma.Educacao.Api.Enumerators;
using System.Net;

namespace Plataforma.Educacao.Api.Controllers;

[ApiController]
public class MainController : ControllerBase
{
    //private readonly IAppIdentityUser _appIdentityUser;
    //private readonly INotificationService _notificationService;

    //public Guid UserId => _appIdentityUser.GetUserId();
    //public bool IsAuthenticated => _appIdentityUser.IsAuthenticated();
    //public string UserEmail => _appIdentityUser.GetUserEmail();

    //protected MainController(IAppIdentityUser appIdentityUser, INotificationService notificationService)
    //{
    //    _appIdentityUser = appIdentityUser;
    //    _notificationService = notificationService;
    //}

    protected ActionResult GenerateResponse(object? result = null,
        ResponseTypeEnum responseType = ResponseTypeEnum.Success,
        HttpStatusCode statusCode = HttpStatusCode.OK,
        IEnumerable<string> errors = null)
    {
        if ((int)statusCode >= 200 && (int)statusCode <= 299)
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

    //protected bool ValidOperation()
    //{
    //    return !_notificationService.HasError();
    //}

    //protected ActionResult GenerateResponse(ModelStateDictionary modelState)
    //{
    //    if (!modelState.IsValid) NotifyInvalidModel(modelState);
    //    return GenerateResponse();
    //}

    //protected void Notify(string message, NotificationTypeEnum type = NotificationTypeEnum.Error)
    //{
    //    _notificationService.Handle(new Notification(message, type));
    //}

    //protected void NotifyInvalidModel(ModelStateDictionary modelState)
    //{
    //    var errors = modelState.Values.SelectMany(e => e.Errors);

    //    foreach (var error in errors)
    //    {
    //        Notify(error?.Exception?.Message ?? error?.ErrorMessage);
    //    }
    //}

    //protected bool ValidateFileType(string fileType)
    //{
    //    if (string.Equals(fileType, "pdf", StringComparison.OrdinalIgnoreCase) || string.Equals(fileType, "xlsx", StringComparison.OrdinalIgnoreCase))
    //    {
    //        return true;
    //    }

    //    Notify("Tipo de arquivo inválido. Use 'pdf' ou 'xlsx'.");
    //    return false;
    //}
}