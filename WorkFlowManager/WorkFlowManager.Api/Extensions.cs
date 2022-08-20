using Microsoft.AspNetCore.Mvc;
using WorkFlowManager.Client;

namespace WorkFlowManager.Api;

public static class Extensions
{
    public static ActionResult ToActionResult<T>(this MethodResult<T> methodResult)
    {
        if (methodResult.IsTimeout)
            return new BadRequestObjectResult("Timeout error!");
        if (!methodResult.IsSuccess)
            return new BadRequestObjectResult(methodResult.Message);
        return new OkObjectResult(methodResult.ReturnValue);
    }
}