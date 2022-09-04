using WorkFlowManager.Shared;
using WorkFlowManager.Shared.Models;
using WorkFlowManager.Core.Repository;

namespace WorkFlowManager.Core;

public class Manager
{
    private readonly ManagerConfiguration _configuration;
    private readonly IRepository _repository;

    public Manager(ManagerConfiguration configuration, IRepository repository)
    {
        _repository = repository;
        _configuration = configuration;
        _repository = repository;
    }

    public MethodResult<List<WorkFlow>> GetWorkFlows(int id = 0, string name = "")
    {
        return MethodResult<List<WorkFlow>>.Ok(_repository.GetWorkFlows(id, name));
    }

    public MethodResult<WorkFlow> AddWorkFlow(string name)
    {
        return MethodResult<WorkFlow>.Ok(_repository.AddWorkFlow(name));
    }

    public MethodResult<Step> AddStartStep(int workFlowId, string name, string description)
    {
        var workFlows = _repository.GetWorkFlows(workFlowId);
        if (workFlows.Count != 1)
            return MethodResult<Step>.Error("Workflow not found!");
        return MethodResult<Step>.Ok(_repository.AddStep(workFlows[0], name, StepTypeEnum.Start, ProcessTypeEnum.None,
            description, "", "", "", ""));
    }

    public MethodResult<Step> AddEndStep(int workFlowId, string name, string description)
    {
        var workFlows = _repository.GetWorkFlows(workFlowId);
        if (workFlows.Count != 1)
            return MethodResult<Step>.Error("Workflow not found!");
        return MethodResult<Step>.Ok(_repository.AddStep(workFlows[0], name, StepTypeEnum.End, ProcessTypeEnum.None,
            description, "", "", "", ""));
    }

    public MethodResult<Step> AddAddOnWorkerStep(int workFlowId, string name, StepTypeEnum stepType, string description,
        string addOnWorkerDllFileName, string addOnWorkerClassName)
    {
        var worker = Extensions.GetWorker(addOnWorkerDllFileName, addOnWorkerClassName);
        if (worker == null)
            return MethodResult<Step>.Error("Plugin not found");
        var workFlows = _repository.GetWorkFlows(workFlowId);
        if (workFlows.Count != 1)
            return MethodResult<Step>.Error("Workflow not found!");
        return MethodResult<Step>.Ok(_repository.AddStep(workFlows[0], name, stepType, ProcessTypeEnum.AddOnWorker,
            description, "", "", addOnWorkerDllFileName, addOnWorkerClassName));
    }

    public MethodResult<Step> AddStarterUserRoleCartableStep(int workFlowId, string name, StepTypeEnum stepType,
        string description)
    {
        var workFlows = _repository.GetWorkFlows(workFlowId);
        if (workFlows.Count != 1)
            return MethodResult<Step>.Error("Workflow not found!");
        return MethodResult<Step>.Ok(_repository.AddStep(workFlows[0], name, stepType,
            ProcessTypeEnum.StarterUserOrRole, description, "", "", "", ""));
    }

    public MethodResult<Step> AddCustomUserRoleCartableStep(int workFlowId, string name, StepTypeEnum stepType,
        string description, string customUser, string customRole)
    {
        var workFlows = _repository.GetWorkFlows(workFlowId);
        if (workFlows.Count != 1)
            return MethodResult<Step>.Error("Workflow not found!");
        return MethodResult<Step>.Ok(_repository.AddStep(workFlows[0], name, stepType, ProcessTypeEnum.CustomUserOrRole,
            description, customUser, customRole, "", ""));
    }

    public MethodResult<Flow> AddFlow(int sourceStepId, int destinationStepId, string condition)
    {
        var sourceStep = _repository.GetStepById(sourceStepId);
        if (sourceStep == null)
            return MethodResult<Flow>.Error("Source Step not found!");
        var destinationStep = _repository.GetStepById(destinationStepId);
        if (destinationStep == null)
            return MethodResult<Flow>.Error("Destination Step not found!");
        return MethodResult<Flow>.Ok(_repository.AddFlow(sourceStep, destinationStep, condition));
    }

    public MethodResult<Entity> AddEntity(string json, string starterUser, string starterRole, int workFlowId)
    {
        return MethodResult<Entity>.Ok(_repository.AddEntity(json, starterUser, starterRole));
    }

    public MethodResult<List<CartableItem>> GetUserCartable(string user)
    {
        return MethodResult<List<CartableItem>>.Ok(_repository.GetUserCartable(user));
    }

    public MethodResult<List<CartableItem>> GetRoleCartable(string role)
    {
        return MethodResult<List<CartableItem>>.Ok(_repository.GetRoleCartable(role));
    }

    public MethodResult<List<CartableItem>> GetServiceCartable(string serviceName)
    {
        return MethodResult<List<CartableItem>>.Ok(_repository.GetServiceCartable(serviceName));
    }
}