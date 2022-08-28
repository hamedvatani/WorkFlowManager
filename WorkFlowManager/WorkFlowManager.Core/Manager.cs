using WorkFlowManager.Client;
using WorkFlowManager.Client.Models;
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

    public async Task<MethodResult<List<WorkFlow>>> GetWorkFlowsAsync(int id = 0, string name = "")
    {
        return MethodResult<List<WorkFlow>>.Ok(await _repository.GetWorkFlowsAsync(id, name));
    }

    public async Task<MethodResult<WorkFlow>> AddWorkFlowAsync(string name)
    {
        return MethodResult<WorkFlow>.Ok(await _repository.AddWorkFlowAsync(name));
    }

    public async Task<MethodResult<Step>> AddStartStepAsync(int workFlowId, string name, string description)
    {
        var workFlows = await _repository.GetWorkFlowsAsync(workFlowId);
        if (workFlows.Count != 1)
            return MethodResult<Step>.Error("Workflow not found!");
        return MethodResult<Step>.Ok(await _repository.AddStepAsync(workFlows[0], name, StepTypeEnum.Start,
            ProcessTypeEnum.None, description, "", "", "", ""));
    }

    public async Task<MethodResult<Step>> AddEndStepAsync(int workFlowId, string name, string description)
    {
        var workFlows = await _repository.GetWorkFlowsAsync(workFlowId);
        if (workFlows.Count != 1)
            return MethodResult<Step>.Error("Workflow not found!");
        return MethodResult<Step>.Ok(await _repository.AddStepAsync(workFlows[0], name, StepTypeEnum.End,
            ProcessTypeEnum.None, description, "", "", "", ""));
    }

    public async Task<MethodResult<Step>> AddAddOnWorkerStepAsync(int workFlowId, string name, StepTypeEnum stepType,
        ProcessTypeEnum processType, string description, string addOnWorkerDllFileName, string addOnWorkerClassName)
    {
        var worker = GetWorker(addOnWorkerDllFileName, addOnWorkerClassName);
        if (worker == null)
            return MethodResult<Step>.Error("Plugin not found");
        var workFlows = await _repository.GetWorkFlowsAsync(workFlowId);
        if (workFlows.Count != 1)
            return MethodResult<Step>.Error("Workflow not found!");
        return MethodResult<Step>.Ok(await _repository.AddStepAsync(workFlows[0], name, stepType, processType,
            description, "", "", addOnWorkerDllFileName, addOnWorkerClassName));
    }

    public async Task<MethodResult<Step>> AddCartableStepAsync(int workFlowId, string name, StepTypeEnum stepType,
        ProcessTypeEnum processType, string description, string customUser, string customRole)
    {
        var workFlows = await _repository.GetWorkFlowsAsync(workFlowId);
        if (workFlows.Count != 1)
            return MethodResult<Step>.Error("Workflow not found!");
        return MethodResult<Step>.Ok(await _repository.AddStepAsync(workFlows[0], name, stepType, processType,
            description, customUser, customRole, "", ""));
    }

    public async Task<MethodResult<Flow>> AddFlowAsync(int sourceStepId, int destinationStepId, string condition)
    {
        var sourceStep = await _repository.GetStepByIdAsync(sourceStepId);
        if (sourceStep == null)
            return MethodResult<Flow>.Error("Source Step not found!");
        var destinationStep = await _repository.GetStepByIdAsync(destinationStepId);
        if (destinationStep == null)
            return MethodResult<Flow>.Error("Destination Step not found!");
        return MethodResult<Flow>.Ok(await _repository.AddFlowAsync(sourceStep, destinationStep, condition));
    }

    public async Task<MethodResult<int>> StartWorkFlow(string json, string starterUser, string starterRole,
        int workFlowId)
    {
        var workFlows = await _repository.GetWorkFlowsAsync(workFlowId);
        if (workFlows.Count != 1)
            return MethodResult<int>.Error("WorkFlow not found!");
        var workFlow = workFlows[0];
        if (!workFlow.IsValid())
            return MethodResult<int>.Error(workFlow.GetValidationError());

        var entity = await _repository.AddEntityAsync(json, starterUser, starterRole, EntityStatusEnum.Idle);

        var startStep = workFlow.Steps.FirstOrDefault(s => s.StepType == StepTypeEnum.Start);
        if (startStep == null)
            return MethodResult<int>.Error("Workflow validation error!");

        RunStepAsync(startStep, entity).Start();

        return MethodResult<int>.Ok(entity.Id);
    }

    public async Task ServiceCallBackAsync(int entityId, int stepId, bool success, string result)
    {
        // var entity = await _repository.GetEntityByIdAsync(entityId);
        // if (entity == null)
        //     return;
        // var step = await _repository.GetStepByIdAsync(stepId);
        // if (step == null)
        //     return;
        // if (!success)
        // {
        //     await _repository.ChangeStatusAsync(entity, step, EntityStatusEnum.FailInStep, result);
        //     await _repository.AddEntityLogAsync(entity, step, EntityLogTypeEnum.ServiceFailed, "", result);
        //     return;
        // }
        //
        // await _repository.AddEntityLogAsync(entity, step, EntityLogTypeEnum.ServiceSucceed, "", result);
        // var nextStep = GetNextStep(step, result);
        // if (nextStep == null)
        // {
        //     await _repository.AddEntityLogAsync(entity, step, EntityLogTypeEnum.ServiceFailed, "Next step not found!",
        //         "");
        //     return;
        // }
        //
        // await RunStepAsync(nextStep, entity);
    }

    public async Task UserRoleCartableCallBackAsync(int cartableId, string result)
    {
        // var cartable = await _repository.GetUserRoleCartableByIdAsync(cartableId);
        // if (cartable == null)
        //     return;
        // var entity = await _repository.GetEntityByIdAsync(cartable.EntityId);
        // if (entity == null)
        //     return;
        // var step = await _repository.GetStepByIdAsync(cartable.StepId);
        // if (step == null)
        //     return;
        //
        // await _repository.AddEntityLogAsync(entity, step, EntityLogTypeEnum.CartableSucceed, "", result);
        // var nextStep = GetNextStep(step, result);
        // if (nextStep == null)
        // {
        //     await _repository.AddEntityLogAsync(entity, step, EntityLogTypeEnum.ServiceFailed, "Next step not found!",
        //         "");
        //     return;
        // }
        //
        // await RunStepAsync(nextStep, entity);
    }

    private async Task RunStepAsync(Step step, Entity entity)
    {
        if (step.StepType == StepTypeEnum.End)
        {
            await _repository.ChangeStatusAsync(entity, step, EntityStatusEnum.End, "");
            await _repository.AddEntityLogAsync(entity, step, EntityLogTypeEnum.GeneralSucceed, "End", "");
            return;
        }

        await _repository.ChangeStatusAsync(entity, step, EntityStatusEnum.RunningStep, "");
        await _repository.AddEntityLogAsync(entity, step, EntityLogTypeEnum.StartStep, "Start Running Step", "");
        Step? nextStep;
        switch (step.ProcessType)
        {
            case ProcessTypeEnum.AddOnWorker:
                var worker = GetStepWorker(step);
                if (worker == null)
                {
                    await _repository.AddEntityLogAsync(entity, step, EntityLogTypeEnum.AddOnFailed,
                        "Worker not found!",
                        "");
                    return;
                }

                string result;
                try
                {
                    result = await worker.RunWorkerAsync(entity);
                }
                catch (Exception e)
                {
                    await _repository.AddEntityLogAsync(entity, step, EntityLogTypeEnum.AddOnFailed, "Worker Exception",
                        e.Message);
                    return;
                }

                await _repository.AddEntityLogAsync(entity, step, EntityLogTypeEnum.AddOnSucceed, "", "");

                nextStep = GetNextStep(step, result);
                if (nextStep == null)
                {
                    await _repository.AddEntityLogAsync(entity, step, EntityLogTypeEnum.AddOnFailed,
                        "Next step not found!", "");
                    return;
                }

                await RunStepAsync(nextStep, entity);
                break;
            case ProcessTypeEnum.Service:
                var serviceName = GetStepServiceName(step);
                await _repository.ChangeStatusAsync(entity, step, EntityStatusEnum.WaitForService, serviceName);
                await _repository.AddServiceCartableAsync(entity, step, step.ServiceName, GetStepPossibleActions(step));
                break;
            case ProcessTypeEnum.StarterUserOrRole:
                await _repository.ChangeStatusAsync(entity, step, EntityStatusEnum.WaitForCartable,
                    "StarterUserOrRole");
                await _repository.AddUserRoleCartableAsync(entity, step, entity.StarterUser, entity.StarterRole,
                    GetStepPossibleActions(step));
                break;
            case ProcessTypeEnum.CustomUserOrRole:
                await _repository.ChangeStatusAsync(entity, step, EntityStatusEnum.WaitForCartable, "CustomUserOrRole");
                await _repository.AddUserRoleCartableAsync(entity, step, step.CustomUser, step.CustomRole,
                    GetStepPossibleActions(step));
                break;
            case ProcessTypeEnum.None:
                nextStep = GetNextStep(step, "");
                if (nextStep == null)
                {
                    await _repository.AddEntityLogAsync(entity, step, EntityLogTypeEnum.GeneralFailed,
                        "Next step not found!", "");
                    return;
                }

                await _repository.AddEntityLogAsync(entity, step, EntityLogTypeEnum.GeneralSucceed, "", "");
                await RunStepAsync(nextStep, entity);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private static Step? GetNextStep(Step step, string condition)
    {
        var flow = step.Heads.FirstOrDefault(f => f.Condition == condition);
        return flow?.DestinationStep;
    }

    private IWorker? GetWorker(string addOnWorkerDllFileName, string addOnWorkerClassName)
    {
        throw new NotImplementedException();
    }

    private IWorker? GetStepWorker(Step step)
    {
        throw new NotImplementedException();
    }

    private string GetStepServiceName(Step step)
    {
        throw new NotImplementedException();
    }

    private string GetStepPossibleActions(Step step)
    {
        var actions = step.Heads.Select(x => x.Condition).Distinct().ToList();
        return actions.Count == 0 ? "" : string.Join(";", actions);
    }
}