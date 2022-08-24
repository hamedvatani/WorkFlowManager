using WorkFlowManager.Client.Models;

namespace WorkFlowManager.Core.Repository;

public class TestRepository : IRepository
{
    private readonly List<WorkFlow> _workFlows = new();
    private readonly List<Entity> _entities = new();
    private readonly List<EntityLog> _entityLogs = new();

    private int _lastWorkFlowId;
    private int _lastStepId;
    private int _lastFlowId;
    private int _lastEntityId;
    private int _lastEntityLogId;

    public List<WorkFlow> GetWorkFlows(int id = 0, string name = "")
    {
        return _workFlows.Where(w =>
                id > 0 ? w.Id == id : name == "" || w.Name == name)
            .ToList();
    }

    public WorkFlow AddWorkFlow(string name)
    {
        _lastWorkFlowId++;
        var workFlow = new WorkFlow
        {
            Id = _lastWorkFlowId,
            Name = name,
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

    public Entity AddEntity(string json, string starterUser, string starterRole)
    {
        _lastEntityId++;
        var entity = new Entity
        {
            Id = _lastEntityId,
            Json = json,
            StarterUser = starterUser,
            StarterRole = starterRole,
            EntityLogs = new List<EntityLog>()
        };
        _entities.Add(entity);
        return entity;
    }

    public EntityLog AddEntityLog(Entity entity, DateTime timeStamp, EntityLogSeverityEnum severity, string subject,
        string description)
    {
        _lastEntityLogId++;
        var entityLog = new EntityLog
        {
            Id = _lastEntityLogId,
            EntityId = entity.Id,
            TimeStamp = timeStamp,
            Severity = severity,
            Subject = subject,
            Description = description,
            Entity = entity
        };
        _entityLogs.Add(entityLog);
        return entityLog;
    }

    public Entity? GetEntityById(int id)
    {
        return _entities.FirstOrDefault(e => e.Id == id);
    }
}