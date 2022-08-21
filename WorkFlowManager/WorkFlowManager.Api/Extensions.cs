using Microsoft.AspNetCore.Mvc;
using WorkFlowManager.Client;

namespace WorkFlowManager.Api;

public static class Extensions
{
    public static ActionResult<T> ToActionResult<T>(this MethodResult<T> methodResult)
    {
        if (methodResult.IsTimeout)
            return new BadRequestObjectResult("Timeout error!");
        if (!methodResult.IsSuccess)
            return new BadRequestObjectResult(methodResult.Message);
        if (methodResult.ReturnValue == null)
            return new OkResult();
        return new OkObjectResult(methodResult.ReturnValue);
    }

    public static ActionResult<T2> ToActionResult<T1, T2>(this MethodResult<T1> methodResult, Func<T1, T2> convertFunc)
    {
        if (methodResult.IsTimeout)
            return new BadRequestObjectResult("Timeout error!");
        if (!methodResult.IsSuccess)
            return new BadRequestObjectResult(methodResult.Message);
        if (methodResult.ReturnValue == null)
            return new OkResult();
        return new OkObjectResult(convertFunc.Invoke(methodResult.ReturnValue));
    }
}