using Microsoft.Extensions.Hosting;
using WorkFlowManager.Client;
using WorkFlowManager.Client.Models;
using WorkFlowManager.Core.Repository;

namespace WorkFlowManager.Core;

public class Manager : IHostedService
{
    private readonly ManagerConfiguration _configuration;
    private readonly IRepository _repository;

    public Manager(ManagerConfiguration configuration, IRepository repository)
    {
        _repository = repository;
        _configuration = configuration;
        _repository = repository;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public MethodResult<List<WorkFlow>> GetWorkFlows(int id = 0, string name = "")
    {
        return MethodResult<List<WorkFlow>>.Ok(_repository.GetWorkFlows(id, name));
    }

    public MethodResult<WorkFlow> AddWorkFlow(string name, string entityName)
    {
        return MethodResult<WorkFlow>.Ok(_repository.AddWorkFlow(name, entityName));
    }

    public MethodResult<Step> AddStep(int workFlowId, string name, StepTypeEnum stepType, ProcessTypeEnum processType,
        string description, string customUser, string customRole)
    {
        var workFlows = _repository.GetWorkFlows(workFlowId);
        if (workFlows.Count != 1)
            return MethodResult<Step>.Error("Workflow not found!");
        return MethodResult<Step>.Ok(_repository.AddStep(workFlows[0], name, stepType, processType, description,
            customUser, customRole));
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


    //
    // public MethodResult ValidateWorkFlow(WorkFlow workFlow)
    // {
    //     if (workFlow.Steps.Count(step => step.StepType == StepTypeEnum.Start) != 1)
    //         return MethodResult.Error("Workflow has to have exact one start step");
    //     var start = workFlow.Steps.FirstOrDefault(step => step.StepType == StepTypeEnum.Start);
    //     if (start == null)
    //         return MethodResult.Error("");
    //     if (start.Tails.Count > 0)
    //         return MethodResult.Error("There is a flow to start step");
    //     if (start.ProcessType != ProcessTypeEnum.None)
    //         return MethodResult.Error("Start step process type must be None");
    //
    //     if (workFlow.Steps.Count(step => step.StepType == StepTypeEnum.End) != 1)
    //         return MethodResult.Error("Workflow has to have exact one end step");
    //     var end = workFlow.Steps.FirstOrDefault(step => step.StepType == StepTypeEnum.End);
    //     if (end == null)
    //         return MethodResult.Error("");
    //     if (end.Heads.Count > 0)
    //         return MethodResult.Error("There is a flow from end step");
    //     if (end.ProcessType != ProcessTypeEnum.None)
    //         return MethodResult.Error("End step process type must be None");
    //
    //     foreach (var step in workFlow.Steps)
    //     {
    //         if (step.Heads.Count(flow => flow.Condition == "") > 1)
    //             return MethodResult.Error($"Step {step.Name} has more than one unconditional flow");
    //         if (step.StepType != StepTypeEnum.Start && step.Tails.Count == 0)
    //             return MethodResult.Error($"There is no flow to step {step.Name}");
    //         if (step.StepType != StepTypeEnum.End && step.Heads.Count == 0)
    //             return MethodResult.Error($"There is no flow from step {step.Name}");
    //         if (step.StepType == StepTypeEnum.Start || step.StepType == StepTypeEnum.Process)
    //         {
    //             if (step.Heads.Count != 1)
    //                 return MethodResult.Error($"step {step.Name} has to have exact one unconditional flow out");
    //             var f = step.Heads.ToList()[0];
    //             if (f.Condition != "")
    //                 return MethodResult.Error($"step {step.Name} has to have exact one unconditional flow out");
    //         }
    //     }
    //
    //     return MethodResult.Ok();
    // }
    //
    // private void RunStep(Step step)
    // {
    //     if (step.StepType == StepTypeEnum.Process || step.StepType == StepTypeEnum.Condition)
    //     {
    //         switch (step.ProcessType)
    //         {
    //             case ProcessTypeEnum.AddOnWorker:
    //                 break;
    //             case ProcessTypeEnum.Service:
    //                 break;
    //             case ProcessTypeEnum.StarterUser:
    //                 break;
    //             case ProcessTypeEnum.StarterRole:
    //                 break;
    //             case ProcessTypeEnum.CustomUser:
    //                 break;
    //             case ProcessTypeEnum.CustomRole:
    //                 break;
    //             case ProcessTypeEnum.None:
    //                 break;
    //             default:
    //                 throw new ArgumentOutOfRangeException();
    //         }
    //     }
    // }
}