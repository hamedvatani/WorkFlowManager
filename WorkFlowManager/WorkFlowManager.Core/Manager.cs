using System.Reflection;
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
        var worker = GetWorker(addOnWorkerDllFileName, addOnWorkerClassName);
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

    public MethodResult<int> StartWorkFlow(string json, string starterUser, string starterRole, int workFlowId)
    {
        var workFlows = _repository.GetWorkFlows(workFlowId);
        if (workFlows.Count != 1)
            return MethodResult<int>.Error("WorkFlow not found!");
        var workFlow = workFlows[0];
        if (!workFlow.IsValid())
            return MethodResult<int>.Error(workFlow.GetValidationError());

        var entity = _repository.AddEntity(json, starterUser, starterRole, EntityStatusEnum.Idle);

        var startStep = workFlow.Steps.FirstOrDefault(s => s.StepType == StepTypeEnum.Start);
        if (startStep == null)
            return MethodResult<int>.Error("Workflow validation error!");

        RunStep(startStep, entity);

        return MethodResult<int>.Ok(entity.Id);
    }

    public void ServiceCallBack(int entityId, int stepId, bool success, string result)
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

    public void UserRoleCartableCallBack(int cartableId, string result)
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

    private void RunStep(Step step, Entity entity)
    {
        if (step.StepType == StepTypeEnum.End)
        {
            _repository.ChangeStatus(entity, step, EntityStatusEnum.End, "");
            _repository.AddEntityLog(entity, step, EntityLogTypeEnum.GeneralSucceed, "End", "");
            return;
        }

        _repository.ChangeStatus(entity, step, EntityStatusEnum.RunningStep, "");
        _repository.AddEntityLog(entity, step, EntityLogTypeEnum.StartStep, "Start Running Step", "");
        Step? nextStep;
        switch (step.ProcessType)
        {
            case ProcessTypeEnum.AddOnWorker:
                var worker = GetStepWorker(step);
                if (worker == null)
                {
                    _repository.AddEntityLog(entity, step, EntityLogTypeEnum.AddOnFailed, "Worker not found!", "");
                    return;
                }

                string result;
                try
                {
                    result = worker.RunWorker(entity);
                }
                catch (Exception e)
                {
                    _repository.AddEntityLog(entity, step, EntityLogTypeEnum.AddOnFailed, "Worker Exception",
                        e.Message);
                    return;
                }

                _repository.AddEntityLog(entity, step, EntityLogTypeEnum.AddOnSucceed, "AddOn Succeed",
                    $"Result : {result}");

                nextStep = GetNextStep(step, result);
                if (nextStep == null)
                {
                    _repository.AddEntityLog(entity, step, EntityLogTypeEnum.AddOnFailed, "Next step not found!", "");
                    return;
                }

                RunStep(nextStep, entity);
                break;
            case ProcessTypeEnum.Service:
                var serviceName = GetStepServiceName(step);
                _repository.ChangeStatus(entity, step, EntityStatusEnum.WaitForService, serviceName);
                _repository.AddServiceCartable(entity, step, step.ServiceName, GetStepPossibleActions(step));
                break;
            case ProcessTypeEnum.StarterUserOrRole:
                _repository.ChangeStatus(entity, step, EntityStatusEnum.WaitForCartable, "StarterUserOrRole");
                _repository.AddUserRoleCartable(entity, step, entity.StarterUser, entity.StarterRole,
                    GetStepPossibleActions(step));
                break;
            case ProcessTypeEnum.CustomUserOrRole:
                _repository.ChangeStatus(entity, step, EntityStatusEnum.WaitForCartable, "CustomUserOrRole");
                _repository.AddUserRoleCartable(entity, step, step.CustomUser, step.CustomRole,
                    GetStepPossibleActions(step));
                break;
            case ProcessTypeEnum.None:
                nextStep = GetNextStep(step, "");
                if (nextStep == null)
                {
                    _repository.AddEntityLog(entity, step, EntityLogTypeEnum.GeneralFailed, "Next step not found!", "");
                    return;
                }

                _repository.AddEntityLog(entity, step, EntityLogTypeEnum.GeneralSucceed, "", "");
                RunStep(nextStep, entity);
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
        var filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins", addOnWorkerDllFileName);
        if (!File.Exists(filename))
            return null;
        var assembly = Assembly.LoadFile(filename);
        var type = assembly.GetType(addOnWorkerClassName);
        if (type == null)
            return null;
        var instance = Activator.CreateInstance(type);
        return (IWorker?) instance;
    }

    private IWorker? GetStepWorker(Step step)
    {
        return GetWorker(step.AddOnWorkerDllFileName, step.AddOnWorkerClassName);
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