using Microsoft.EntityFrameworkCore;
using WorkFlowManager.Client.Models;
using WorkFlowManager.Core.Data;

namespace WorkFlowManager.Core.Repository;

public class WfmRepository : IRepository
{
    private readonly WorkFlowManagerContext _context;

    public WfmRepository(WorkFlowManagerContext context)
    {
        _context = context;
    }

    public Task<List<WorkFlow>> GetWorkFlowsAsync(int id = 0, string name = "")
    {
        return _context.WorkFlows.Where(w => id > 0 ? w.Id == id : name == "" || w.Name == name).ToListAsync();
    }

    public Task<WorkFlow> AddWorkFlowAsync(string name)
    {
        var workFlow = new WorkFlow { Name = name };
        _context.WorkFlows.Add(workFlow);
        _context.SaveChangesAsync();
        return Task.FromResult(workFlow);
    }

    public Task<Step> AddStepAsync(WorkFlow workFlow, string name, StepTypeEnum stepType,
        ProcessTypeEnum processType, string description, string customUser, string customRole)
    {
        var step = new Step
        {
            WorkFlow = workFlow,
            Name = name,
            StepType = stepType,
            ProcessType = processType,
            Description = description,
            CustomUser = customUser,
            CustomRole = customRole
        };
        _context.Steps.Add(step);
        _context.SaveChangesAsync();
        return Task.FromResult(step);
    }

    public Task<Step?> GetStepByIdAsync(int id)
    {
        return _context.Steps.FirstOrDefaultAsync(s => s.Id == id);
    }

    public Task<Flow> AddFlowAsync(Step sourceStep, Step destinationStep, string condition)
    {
        var flow = new Flow
        {
            SourceStep = sourceStep,
            DestinationStep = destinationStep,
            Condition = condition
        };
        _context.Flows.Add(flow);
        _context.SaveChangesAsync();
        return Task.FromResult(flow);
    }

    public Task<Entity> AddEntityAsync(string json, string starterUser, string starterRole, EntityStatusEnum status)
    {
        var entity = new Entity
        {
            Json=json,
            StarterUser = starterUser,
            StarterRole = starterRole,
            Status = status
        };
        _context.Entities.Add(entity);
        _context.SaveChangesAsync();
        return Task.FromResult(entity);
    }

    public Task ChangeStatusAsync(Entity entity, Step step, EntityStatusEnum status)
    {
        entity.CurrentStep = step;
        entity.Status = status;
        _context.SaveChangesAsync();
        return Task.CompletedTask;
    }

    public Task<EntityLog> AddEntityLogAsync(Entity entity, Step? step, EntityLogTypeEnum logType, string subject, string description)
    {
        var entityLog = new EntityLog
        {
            Entity = entity,
            Step = step,
            LogType = logType,
            Subject = subject,
            Description = description
        };
        _context.SaveChangesAsync();
        return Task.FromResult(entityLog);
    }
}