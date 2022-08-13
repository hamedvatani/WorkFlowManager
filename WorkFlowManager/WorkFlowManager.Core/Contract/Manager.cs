using WorkFlowManager.Core.Models;
using WorkFlowManager.Core.Repository;

namespace WorkFlowManager.Core.Contract;

public class Manager : IManager
{
    private readonly IRepository _repository;

    public Manager(IRepository repository)
    {
        _repository = repository;
    }

    public WorkFlow? GetWorkFlow(string name)
    {
        return _repository.GetWorkFlow(name);
    }

    public WorkFlow AddWorkFlow(string name, string entityName)
    {
        return _repository.AddWorkFlow(name, entityName);
    }

    public Step AddStep(WorkFlow workFlow, string name, StepTypeEnum stepType, ProcessTypeEnum processType,
        string description, string customUser, string customRole, AddOnWorker? addOnWorker)
    {
        return _repository.AddStep(workFlow, name, stepType, processType, description, customUser, customRole,
            addOnWorker);
    }

    public Flow AddFlow(Step sourceStep, Step destinationStep, string condition)
    {
        return _repository.AddFlow(sourceStep, destinationStep, condition);
    }

    public MethodResult ValidateWorkFlow(WorkFlow workFlow)
    {
        if (workFlow.Steps.Count(step => step.StepType == StepTypeEnum.Start) != 1)
            return MethodResult.Error("Workflow has to have exact one start step");
        var start = workFlow.Steps.FirstOrDefault(step => step.StepType == StepTypeEnum.Start);
        if (start == null)
            return MethodResult.Error("");
        if (start.Tails.Count > 0)
            return MethodResult.Error("There is a flow to start step");
        if (start.ProcessType != ProcessTypeEnum.None)
            return MethodResult.Error("Start step process type must be None");

        if (workFlow.Steps.Count(step => step.StepType == StepTypeEnum.End) != 1)
            return MethodResult.Error("Workflow has to have exact one end step");
        var end = workFlow.Steps.FirstOrDefault(step => step.StepType == StepTypeEnum.End);
        if (end == null)
            return MethodResult.Error("");
        if (end.Heads.Count > 0)
            return MethodResult.Error("There is a flow from end step");
        if (end.ProcessType != ProcessTypeEnum.None)
            return MethodResult.Error("End step process type must be None");

        foreach (var step in workFlow.Steps)
        {
            if (step.Heads.Count(flow => flow.Condition == "") > 1)
                return MethodResult.Error($"Step {step.Name} has more than one unconditional flow");
            if (step.StepType != StepTypeEnum.Start && step.Tails.Count == 0)
                return MethodResult.Error($"There is no flow to step {step.Name}");
            if (step.StepType != StepTypeEnum.End && step.Heads.Count == 0)
                return MethodResult.Error($"There is no flow from step {step.Name}");
            if (step.StepType == StepTypeEnum.Start || step.StepType == StepTypeEnum.Process)
            {
                if (step.Heads.Count != 1)
                    return MethodResult.Error($"step {step.Name} has to have exact one unconditional flow out");
                var f = step.Heads.ToList()[0];
                if (f.Condition != "")
                    return MethodResult.Error($"step {step.Name} has to have exact one unconditional flow out");
            }
        }

        return MethodResult.Ok();
    }

    public MethodResult StartWorkFlow(IEntity entity, WorkFlow workFlow)
    {
        if (workFlow.EntityName != entity.Name)
            return MethodResult.Error("entity name not match!");
        var res = ValidateWorkFlow(workFlow);
        if (!res.IsSuccess)
            return res;

        var startStep = workFlow.Steps.FirstOrDefault(s => s.StepType == StepTypeEnum.Start);
        if (startStep == null)
            return MethodResult.Error("");

        RunStep(startStep);
        return MethodResult.Ok();
    }

    private void RunStep(Step step)
    {
        if (step.StepType == StepTypeEnum.Process || step.StepType == StepTypeEnum.Condition)
        {
            switch (step.ProcessType)
            {
                case ProcessTypeEnum.AddOnWorker:
                    break;
                case ProcessTypeEnum.Service:
                    break;
                case ProcessTypeEnum.StarterUser:
                    break;
                case ProcessTypeEnum.StarterRole:
                    break;
                case ProcessTypeEnum.CustomUser:
                    break;
                case ProcessTypeEnum.CustomRole:
                    break;
                case ProcessTypeEnum.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}