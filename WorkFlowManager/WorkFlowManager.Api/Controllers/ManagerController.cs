using Microsoft.AspNetCore.Mvc;
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
    public async Task<ActionResult<List<WorkFlowDto>>> GetWorkFlows([FromBody] GetWorkFlowsDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return (await _manager.GetWorkFlowsAsync(model.Id, model.Name))
            .ToActionResult(x => x.Select(y => new WorkFlowDto(y)).ToList());
    }

    [HttpPost("AddWorkFlow")]
    public async Task<ActionResult<WorkFlowDto>> AddWorkFlow([FromBody] AddWorkFlowDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return (await _manager.AddWorkFlowAsync(model.Name)).ToActionResult(x => new WorkFlowDto(x));
    }

    [HttpPost("AddStartStep")]
    public async Task<ActionResult<StepDto>> AddStartStep([FromBody] AddStartStepDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return (await _manager.AddStartStepAsync(model.WorkFlowId, model.Name, model.Description)).ToActionResult(x =>
            new StepDto(x));
    }

    [HttpPost("AddEndStep")]
    public async Task<ActionResult<StepDto>> AddEndStep([FromBody] AddEndStepDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return (await _manager.AddStartStepAsync(model.WorkFlowId, model.Name, model.Description)).ToActionResult(x =>
            new StepDto(x));
    }

    [HttpPost("AddAddOnWorkerStep")]
    public async Task<ActionResult<StepDto>> AddAddOnWorkerStep([FromBody] AddAddOnWorkerStepDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return (await _manager.AddAddOnWorkerStepAsync(model.WorkFlowId, model.Name, model.StepType, model.ProcessType,
                model.Description, model.AddOnWorkerDllFileName, model.AddOnWorkerClassName))
            .ToActionResult(x => new StepDto(x));
    }

    [HttpPost("AddCartableStep")]
    public async Task<ActionResult<StepDto>> AddCartableStep([FromBody] AddCartableStepDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return (await _manager.AddCartableStepAsync(model.WorkFlowId, model.Name, model.StepType, model.ProcessType,
            model.Description, model.CustomUser, model.CustomRole)).ToActionResult(x => new StepDto(x));
    }

    [HttpPost("AddFlow")]
    public async Task<ActionResult<FlowDto>> AddFlow([FromBody] AddFlowDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return (await _manager.AddFlowAsync(model.SourceStepId, model.DestinationStepId, model.Condition))
            .ToActionResult(x => new FlowDto(x));
    }
}