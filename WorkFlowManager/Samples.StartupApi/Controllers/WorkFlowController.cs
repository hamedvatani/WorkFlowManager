using Microsoft.AspNetCore.Mvc;
using Samples.ShoppigCard;
using WorkFlowManager.Core.Interfaces;
using WorkFlowManager.Core.Models;

namespace Samples.StartupApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WorkFlowController : ControllerBase
{
    private readonly ILogger<WorkFlowController> _logger;
    private readonly IWorkFlowBiz biz;

    public WorkFlowController(ILogger<WorkFlowController> logger, IWorkFlowBiz biz)
    {
        _logger = logger;
        this.biz = biz;
    }

    [HttpGet]
    [Route("CreateWorkFlow")]
    public ActionResult CreateWorkFlow()
    {
        biz.CreateMyWorkFlow();
        return Ok();
    }
}