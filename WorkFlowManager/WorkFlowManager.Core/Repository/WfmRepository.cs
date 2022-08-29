using Microsoft.EntityFrameworkCore;
using WorkFlowManager.Shared.Models;
using WorkFlowManager.Shared.Data;

namespace WorkFlowManager.Core.Repository;

public class WfmRepository : IRepository
{
    private readonly WorkFlowManagerContext _context;

    public WfmRepository(WorkFlowManagerContext context)
    {
        _context = context;
    }

    public List<WorkFlow> GetWorkFlows(int id = 0, string name = "")
    {
        return _context.WorkFlows.Where(w => id > 0 ? w.Id == id : name == "" || w.Name == name)
            .Include(x => x.Steps)
            .ThenInclude(x => x.Heads)
            .Include(x => x.Steps)
            .ThenInclude(x => x.Tails)
            .ToList();
    }

    public WorkFlow AddWorkFlow(string name)
    {
        var workFlow = new WorkFlow {Name = name};
        _context.WorkFlows.Add(workFlow);
        _context.SaveChanges();
        return workFlow;
    }

    public Step AddStep(WorkFlow workFlow, string name, StepTypeEnum stepType, ProcessTypeEnum processType,
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
        _context.SaveChanges();
        return step;
    }

    public Step? GetStepById(int id)
    {
        return _context.Steps.FirstOrDefault(s => s.Id == id);
    }

    public Flow AddFlow(Step sourceStep, Step destinationStep, string condition)
    {
        var flow = new Flow
        {
            SourceStep = sourceStep,
            DestinationStep = destinationStep,
            Condition = condition
        };
        _context.Flows.Add(flow);
        _context.SaveChanges();
        return flow;
    }

    public Entity AddEntity(string json, string starterUser, string starterRole, EntityStatusEnum status)
    {
        var entity = new Entity
        {
            Json = json,
            StarterUser = starterUser,
            StarterRole = starterRole,
            Status = status
        };
        _context.Entities.Add(entity);
        _context.SaveChanges();
        return entity;
    }

    public Entity? GetEntityById(int id)
    {
        return _context.Entities.FirstOrDefault(e => e.Id == id);
    }

    public void ChangeStatus(Entity entity, Step step, EntityStatusEnum status, string description)
    {
        entity.CurrentStep = step;
        entity.Status = status;
        entity.Description = description;
        _context.SaveChanges();
    }

    public EntityLog AddEntityLog(Entity entity, Step? step, EntityLogTypeEnum logType, string subject,
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
        _context.EntityLogs.Add(entityLog);
        _context.SaveChanges();
        return entityLog;
    }

    public UserRoleCartable AddUserRoleCartable(Entity entity, Step step, string user, string role,
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
        _context.SaveChanges();
        return cartable;
    }

    public bool DeleteUserRoleCartable(int id)
    {
        var cartable = _context.UserRoleCartables.FirstOrDefault(c => c.Id == id);
        if (cartable == null)
            return false;
        _context.UserRoleCartables.Remove(cartable);
        _context.SaveChanges();
        return true;
    }

    public UserRoleCartable? GetUserRoleCartableById(int id)
    {
        return _context.UserRoleCartables.FirstOrDefault(c => c.Id == id);
    }

    public List<UserRoleCartable> GetUserCartables(string user)
    {
        return _context.UserRoleCartables.Where(c => c.User == user).ToList();
    }

    public List<UserRoleCartable> GetRoleCartables(string role)
    {
        return _context.UserRoleCartables.Where(c => c.Role == role).ToList();
    }

    public ServiceCartable AddServiceCartable(Entity entity, Step step, string serviceName, string possibleActions)
    {
        var cartable = new ServiceCartable
        {
            Entity = entity,
            Step = step,
            ServiceName = serviceName,
            PossibleActions = possibleActions
        };
        _context.ServiceCartables.Add(cartable);
        _context.SaveChanges();
        return cartable;
    }

    public bool DeleteServiceCartable(int id)
    {
        var cartable = _context.ServiceCartables.FirstOrDefault(c => c.Id == id);
        if (cartable == null)
            return false;
        _context.ServiceCartables.Remove(cartable);
        _context.SaveChanges();
        return true;
    }

    public ServiceCartable? GetServiceCartableById(int id)
    {
        return _context.ServiceCartables.FirstOrDefault(c => c.Id == id);
    }

    public List<ServiceCartable> GetServiceCartables(string serviceName)
    {
        return _context.ServiceCartables.Where(c => c.ServiceName == serviceName).ToList();
    }
}