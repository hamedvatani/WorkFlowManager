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
        var workFlow = new WorkFlow {Name = name};
        _context.WorkFlows.Add(workFlow);
        _context.SaveChangesAsync();
        return Task.FromResult(workFlow);
    }

    public Task<Step> AddStepAsync(WorkFlow workFlow, string name, StepTypeEnum stepType, ProcessTypeEnum processType,
        string description, string customUser, string customRole, string addOnWorkerDllFileName,
        string addOnWorkerClassName)
    {
        var step = new Step
        {
            WorkFlow = workFlow,
            Name = name,
            StepType = stepType,
            ProcessType = processType,
            Description = description,
            CustomUser = customUser,
            CustomRole = customRole,
            AddOnWorkerDllFileName = addOnWorkerDllFileName,
            AddOnWorkerClassName = addOnWorkerClassName
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
            Json = json,
            StarterUser = starterUser,
            StarterRole = starterRole,
            Status = status
        };
        _context.Entities.Add(entity);
        _context.SaveChangesAsync();
        return Task.FromResult(entity);
    }

    public Task<Entity?> GetEntityByIdAsync(int id)
    {
        return _context.Entities.FirstOrDefaultAsync(e => e.Id == id);
    }

    public Task ChangeStatusAsync(Entity entity, Step step, EntityStatusEnum status, string description)
    {
        entity.CurrentStep = step;
        entity.Status = status;
        entity.Description = description;
        _context.SaveChangesAsync();
        return Task.CompletedTask;
    }

    public Task<EntityLog> AddEntityLogAsync(Entity entity, Step? step, EntityLogTypeEnum logType, string subject,
        string description)
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

    public Task<UserRoleCartable> AddUserRoleCartableAsync(Entity entity, Step step, string user, string role,
        string possibleActions)
    {
        var cartable = new UserRoleCartable
        {
            Entity = entity,
            Step = step,
            User = user,
            Role = role,
            PossibleActions = possibleActions
        };
        _context.UserRoleCartables.Add(cartable);
        _context.SaveChangesAsync();
        return Task.FromResult(cartable);
    }

    public Task<bool> DeleteUserRoleCartableAsync(int id)
    {
        var cartable = _context.UserRoleCartables.FirstOrDefault(c => c.Id == id);
        if (cartable == null)
            return Task.FromResult(false);
        _context.UserRoleCartables.Remove(cartable);
        _context.SaveChangesAsync();
        return Task.FromResult(true);
    }

    public Task<UserRoleCartable?> GetUserRoleCartableByIdAsync(int id)
    {
        return _context.UserRoleCartables.FirstOrDefaultAsync(c => c.Id == id);
    }

    public Task<List<UserRoleCartable>> GetUserCartablesAsync(string user)
    {
        return _context.UserRoleCartables.Where(c => c.User == user).ToListAsync();
    }

    public Task<List<UserRoleCartable>> GetRoleCartablesAsync(string role)
    {
        return _context.UserRoleCartables.Where(c => c.Role == role).ToListAsync();
    }

    public Task<ServiceCartable> AddServiceCartableAsync(Entity entity, Step step, string serviceName,
        string possibleActions)
    {
        var cartable = new ServiceCartable
        {
            Entity = entity,
            Step = step,
            ServiceName = serviceName,
            PossibleActions = possibleActions
        };
        _context.ServiceCartables.Add(cartable);
        _context.SaveChangesAsync();
        return Task.FromResult(cartable);
    }

    public Task<bool> DeleteServiceCartableAsync(int id)
    {
        var cartable = _context.ServiceCartables.FirstOrDefault(c => c.Id == id);
        if (cartable == null)
            return Task.FromResult(false);
        _context.ServiceCartables.Remove(cartable);
        _context.SaveChangesAsync();
        return Task.FromResult(true);
    }

    public Task<ServiceCartable?> GetServiceCartableByIdAsync(int id)
    {
        return _context.ServiceCartables.FirstOrDefaultAsync(c => c.Id == id);
    }

    public Task<List<ServiceCartable>> GetServiceCartablesAsync(string serviceName)
    {
        return _context.ServiceCartables.Where(c => c.ServiceName == serviceName).ToListAsync();
    }
}