using WorkFlowManager.Core.Data;
using WorkFlowManager.Core.Models;

namespace WorkFlowManager.Core.Repository;

public class TestRepository : IRepository
{
    // private int _lastWorkerId;
    // private int _lastWorkFlowId;
    // private int _lastStepId;
    // private int _lastFlowId;
    //
    // public AddOnWorker AddWorker(string fileName, string className)
    // {
    //     _lastWorkerId++;
    //     var worker = new AddOnWorker
    //     {
    //         Id = _lastWorkerId,
    //         FileName = fileName,
    //         ClassName = className
    //     };
    //     return worker;
    // }
    //
    // public WorkFlow? GetWorkFlow(string name)
    // {
    //     return null;
    // }
    //
    // public WorkFlow AddWorkFlow(string name, string entityName)
    // {
    //     _lastWorkFlowId++;
    //     var workFlow = new WorkFlow
    //     {
    //         Id = _lastWorkFlowId,
    //         Name = name,
    //         EntityName = entityName,
    //         Steps = new List<Step>()
    //     };
    //     return workFlow;
    // }
    //
    // public Step AddStep(WorkFlow workFlow, string name, StepTypeEnum stepType, ProcessTypeEnum processType,
    //     string description, string customUser, string customRole, AddOnWorker? addOnWorker)
    // {
    //     _lastStepId++;
    //     var step = new Step
    //     {
    //         Id = _lastStepId,
    //         StepType = stepType,
    //         Name = name,
    //         Description = description,
    //         ProcessType = processType,
    //         CustomUser = customUser,
    //         CustomRole = customRole,
    //         WorkFlowId = workFlow.Id,
    //         WorkFlow = workFlow,
    //         AddOnWorkerId = addOnWorker?.Id,
    //         AddOnWorker = addOnWorker,
    //         Heads = new List<Flow>(),
    //         Tails = new List<Flow>()
    //     };
    //     workFlow.Steps.Add(step);
    //     return step;
    // }
    //
    // public Flow AddFlow(Step sourceStep, Step destinationStep, string condition)
    // {
    //     _lastFlowId++;
    //     var flow = new Flow
    //     {
    //         Id = _lastFlowId,
    //         Condition = condition,
    //         SourceStepId = sourceStep.Id,
    //         SourceStep = sourceStep,
    //         DestinationStepId = destinationStep.Id,
    //         DestinationStep = destinationStep
    //     };
    //     sourceStep.Heads.Add(flow);
    //     destinationStep.Tails.Add(flow);
    //     return flow;
    // }
}