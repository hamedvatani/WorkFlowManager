using WorkFlowManager.Client.Models;

namespace WorkFlowManager.Core.Repository;

public class TestRepository : IRepository
{
    private readonly List<WorkFlow> _workFlows = new();

    private int _lastWorkFlowId;
    private int _lastStepId;
    private int _lastFlowId;

    public List<WorkFlow> GetWorkFlows(int id = 0, string name = "")
    {
        return _workFlows.Where(w =>
                id > 0 ? w.Id == id : name == "" || w.Name == name)
            .ToList();
    }

    public WorkFlow AddWorkFlow(string name, string entityName)
    {
        _lastWorkFlowId++;
        var workFlow = new WorkFlow
        {
            Id = _lastWorkFlowId,
            Name = name,
            EntityName = entityName,
            Steps = new List<Step>()
        };
        _workFlows.Add(workFlow);
        return workFlow;
    }

    public Step AddStep(WorkFlow workFlow, string name, StepTypeEnum stepType, ProcessTypeEnum processType, string description, string customUser, string customRole)
    {
        _lastStepId++;
        var step = new Step
        {
            Id = _lastStepId,
            StepType = stepType,
            Name = name,
            Description = description,
            ProcessType = processType,
            CustomUser = customUser,
            CustomRole = customRole,
            WorkFlowId = workFlow.Id,
            WorkFlow = workFlow,
            Heads = new List<Flow>(),
            Tails = new List<Flow>()
        };
        workFlow.Steps.Add(step);
        return step;
    }

    public Step? GetStepById(int id)
    {
        return _workFlows.SelectMany(workFlow => workFlow.Steps).FirstOrDefault(step => step.Id == id);
    }

    public Flow AddFlow(Step sourceStep, Step destinationStep, string condition)
    {
        _lastFlowId++;
        var flow = new Flow
        {
            Id = _lastFlowId,
            Condition = condition,
            SourceStepId = sourceStep.Id,
            SourceStep = sourceStep,
            DestinationStepId = destinationStep.Id,
            DestinationStep = destinationStep
        };
        sourceStep.Heads.Add(flow);
        destinationStep.Tails.Add(flow);
        return flow;
    }
}