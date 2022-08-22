using Microsoft.AspNetCore.Mvc;
using WorkFlowManager.Client.Models;
using WorkFlowManager.Client.Models.Dto;
using WorkFlowManager.Core;

namespace WorkFlowManager.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ManagerController : ControllerBase
{
    private readonly Manager _manager;

    public ManagerController(Manager manager)
    {
        _manager = manager;
    }

    [HttpPost("GetWorkFlows")]
    public ActionResult<List<WorkFlowDto>> GetWorkFlows([FromBody] GetWorkFlowsDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return _manager.GetWorkFlows(model.Id, model.Name)
            .ToActionResult(x => x.Select(y => new WorkFlowDto(y)).ToList());
    }

    [HttpPost("AddWorkFlow")]
    public ActionResult<WorkFlowDto> AddWorkFlow([FromBody] AddWorkFlowDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return _manager.AddWorkFlow(model.Name, model.EntityName).ToActionResult(x => new WorkFlowDto(x));
    }

    [HttpPost("AddStep")]
    public ActionResult<StepDto> AddStep([FromBody] AddStepDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return _manager.AddStep(model.WorkFlowId, model.Name, model.StepType, model.ProcessType, model.Description,
            model.CustomUser, model.CustomRole).ToActionResult(x => new StepDto(x));
    }

    [HttpPost("AddFlow")]
    public ActionResult<FlowDto> AddFlow([FromBody] AddFlowDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return _manager.AddFlow(model.SourceStepId, model.DestinationStepId, model.Condition)
            .ToActionResult(x => new FlowDto(x));
    }
}