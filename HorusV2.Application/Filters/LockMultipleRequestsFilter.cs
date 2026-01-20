using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace HorusV2.Application.Filters;

public class LockMultipleRequestsFilter : IAsyncActionFilter
{
    private static bool _isAlreadyProcessing;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (_isAlreadyProcessing)
        {
            context.Result = new StatusCodeResult(StatusCodes.Status429TooManyRequests);

            string requestAudit =
                $"Excesso de requisições identificado no endpoint: {context.RouteData.Values["controller"]}/{context.RouteData.Values["action"]}.";

            Log.Warning(requestAudit);

            return;
        }

        _isAlreadyProcessing = true;

        await next();

        _isAlreadyProcessing = false;
    }
}