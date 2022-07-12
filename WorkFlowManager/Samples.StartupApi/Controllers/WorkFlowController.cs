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
    private readonly IWfManager wfManager;
    private readonly IWorkFlowBiz biz;

    public WorkFlowController(ILogger<WorkFlowController> logger, IWfManager wfManager, IWorkFlowBiz biz)
    {
        _logger = logger;
        this.wfManager = wfManager;
        this.biz = biz;
    }

    [HttpGet]
    [Route("GetAllWorkFlows")]
    public ActionResult<List<WorkFlow>> GetAllWorkFlows()
    {
        return Ok(wfManager.GetAllWorkFlows());
    }

    [HttpGet]
    [Route("GetWorkFlow")]
    public ActionResult<WorkFlow?> GetWorkFlow(string name)
    {
        return Ok(wfManager.GetWorkFlow(name));
    }

    [HttpGet]
    [Route("CreateWorkFlow")]
    public ActionResult CreateWorkFlow()
    {
        biz.CreateMyWorkFlow();
        return Ok();
    }
}