using System.Net;
using HorusV2.Application.DTO.Response.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace HorusV2.Application.Filters;

public class BasicAuthorizationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        /*
         * Verificação básica afim de verificar se o usuário SIGSM foi autenticado 
         * e está enviando as informações necessárias para futura integração ao HÓRUS.
         */
        try
        {
            int sigsmUserId = Convert.ToInt32(context.HttpContext.Request.Headers["SIGSM_USER_ID"]);
            int sigsmIbgeCityCode = Convert.ToInt32(context.HttpContext.Request.Headers["SIGSM_IBGE_CITY_CODE"]);

            if (sigsmUserId > 0 && sigsmIbgeCityCode > 0)
            {
                context.HttpContext.Items.Add("sigsmUserId", sigsmUserId);
                context.HttpContext.Items.Add("sigsmIbgeCityCode", sigsmIbgeCityCode);

                await next();
            }
            else
            {
                context.Result =
                    new UnauthorizedObjectResult(new ErrorResponseDTO(HttpStatusCode.Unauthorized,
                        "Usuário não autorizado."));

                string requestAudit =
                    $"Tentativa de autorização inválida no endpoint: {context.RouteData.Values["controller"]}/{context.RouteData.Values["action"]}.\nUsuário informado: {sigsmUserId}.\nCidade informada: {sigsmIbgeCityCode}.";

                Log.Error(requestAudit);
            }
        }
        catch (Exception ex)
        {
            context.Result =
                new UnauthorizedObjectResult(new ErrorResponseDTO(HttpStatusCode.Unauthorized,
                    "Usuário não autorizado."));

            string requestAudit =
                $"Falha ao tentar autorizar usuário no endpoint: {context.RouteData.Values["controller"]}/{context.RouteData.Values["action"]}.\nErro: {ex.Message}";

            Log.Error(requestAudit);
        }
    }
}