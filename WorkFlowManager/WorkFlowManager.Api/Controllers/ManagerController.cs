using Microsoft.AspNetCore.Mvc;
using WorkFlowManager.Shared.Models.Dto;
using WorkFlowManager.Core;

namespace WorkFlowManager.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ManagerController : ControllerBase
{
    private readonly Manager _manager;
    private readonly ManagerService _managerService;

    public ManagerController(Manager manager, ManagerService managerService)
    {
        _manager = manager;
        _managerService = managerService;
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
        return _manager.AddWorkFlow(model.Name).ToActionResult(x => new WorkFlowDto(x));
    }

    [HttpPost("AddStartStep")]
    public ActionResult<StepDto> AddStartStep([FromBody] AddStartStepDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return _manager.AddStartStep(model.WorkFlowId, model.Name, model.Description)
            .ToActionResult(x => new StepDto(x));
    }

    [HttpPost("AddEndStep")]
    public ActionResult<StepDto> AddEndStep([FromBody] AddEndStepDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return _manager.AddEndStep(model.WorkFlowId, model.Name, model.Description).ToActionResult(x =>
            new StepDto(x));
    }

    [HttpPost("AddAddOnWorkerStep")]
    public ActionResult<StepDto> AddAddOnWorkerStep([FromBody] AddAddOnWorkerStepDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return _manager
            .AddAddOnWorkerStep(model.WorkFlowId, model.Name, model.StepType, model.Description,
                model.AddOnWorkerDllFileName, model.AddOnWorkerClassName).ToActionResult(x => new StepDto(x));
    }

    [HttpPost("AddStarterUserRoleCartableStep")]
    public ActionResult<StepDto> AddStarterUserRoleCartableStep([FromBody] AddStarterUserRoleCartableStepDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return _manager.AddStarterUserRoleCartableStep(model.WorkFlowId, model.Name, model.StepType, model.Description)
            .ToActionResult(x => new StepDto(x));
    }

    [HttpPost("AddCustomUserRoleCartableStep")]
    public ActionResult<StepDto> AddCustomUserRoleCartableStep([FromBody] AddCustomUserRoleCartableStepDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return _manager.AddCustomUserRoleCartableStep(model.WorkFlowId, model.Name, model.StepType, model.Description,
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

    [HttpPost("StartWorkFlow")]
    public ActionResult<EntityDto> StartWorkFlow([FromBody] StartWorkFlowDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var result = _manager.AddEntity(model.Json, model.StarterUser, model.StarterRole, model.WorkFlowId);
        if (result.IsSuccess)
            _managerService.StartWorkFlow(result.GetResult().Id, model.WorkFlowId);
        return result.ToActionResult(x => new EntityDto(x));
    }

    [HttpPost("GetUserCartable")]
    public ActionResult<List<CartableItemDto>> GetUserCartable([FromBody] GetUserCartableDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return _manager.GetUserCartable(model.User).ToActionResult(x => x.Select(y => new CartableItemDto(y)).ToList());
    }

    [HttpPost("GetRoleCartable")]
    public ActionResult<List<CartableItemDto>> GetRoleCartable([FromBody] GetRoleCartableDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return _manager.GetRoleCartable(model.Role).ToActionResult(x => x.Select(y => new CartableItemDto(y)).ToList());
    }

    [HttpPost("GetServiceCartable")]
    public ActionResult<List<CartableItemDto>> GetServiceCartable([FromBody] GetServiceCartableDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return _manager.GetServiceCartable(model.ServiceName)
            .ToActionResult(x => x.Select(y => new CartableItemDto(y)).ToList());
    }
}