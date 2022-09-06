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

    public Step? GetNextStep(int stepId, string result)
    {
        var step = _context.Steps.Include(x => x.Heads).ThenInclude(x => x.DestinationStep)
            .FirstOrDefault(s => s.Id == stepId);
        if (step == null)
            return null;
        var flow = step.Heads.FirstOrDefault(f => f.Condition == result);
        if (flow == null)
            return null;
        return GetStepById(flow.DestinationStepId);
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

    public Entity AddEntity(string json, string starterUser, string starterRole)
    {
        var entity = new Entity
        {
            Status = EntityStatusEnum.Idle,
            Json = json,
            StarterUser = starterUser,
            StarterRole = starterRole
        };
        _context.Entities.Add(entity);
        _context.SaveChanges();
        return entity;
    }

    public void ChangeEntityStatus(Entity entity, EntityStatusEnum status)
    {
        entity.Status = status;
        _context.SaveChanges();
    }

    public Entity? GetEntityById(int id)
    {
        return _context.Entities.FirstOrDefault(e => e.Id == id);
    }

    public EntityLog AddEntityLog(Entity entity, Step step, EntityLogStatusEnum logType, string description)
    {
        var entityLog = new EntityLog
        {
            Entity = entity,
            Step = step,
            LogType = logType,
            Description = description
        };
        _context.EntityLogs.Add(entityLog);
        _context.SaveChanges();
        return entityLog;
    }

    public CartableItem AddCartableItem(Entity entity, Step step, string user, string role, string serviceName,
        string possibleActions)
    {
        var cartableItem = new CartableItem
        {
            Entity = entity,
            Step = step,
            User = user,
            Role = role,
            ServiceName = serviceName,
            PossibleActions = possibleActions
        };
        _context.CartableItems.Add(cartableItem);
        _context.SaveChanges();
        return cartableItem;
    }

    public bool DeleteCartableItem(int id)
    {
        var cartableItem = _context.CartableItems.FirstOrDefault(c => c.Id == id);
        if (cartableItem == null)
            return false;
        _context.CartableItems.Remove(cartableItem);
        _context.SaveChanges();
        return true;
    }

    private IQueryable<CartableItem> GetCartableItemQuery(int id = 0, string user = "", string role = "",
        string serviceName = "")
    {
        return _context.CartableItems
            .Include(x => x.Entity)
            .ThenInclude(x => x.EntityLogs)
            .Include(x => x.Step)
            .ThenInclude(x => x.Heads)
            .Include(x => x.Step)
            .ThenInclude(x => x.Tails)
            .Where(c => (id == 0 || c.Id == id) &&
                        (user == "" || c.User == user) &&
                        (role == "" || c.Role == role) &&
                        (serviceName == "" || c.ServiceName == serviceName));
    }

    public CartableItem? GetCartableItemById(int id)
    {
        return GetCartableItemQuery(id: id).FirstOrDefault();
    }

    public List<CartableItem> GetUserCartable(string user)
    {
        return GetCartableItemQuery(user: user).ToList();
    }

    public List<CartableItem> GetRoleCartable(string role)
    {
        return GetCartableItemQuery(role: role).ToList();
    }

    public List<CartableItem> GetServiceCartable(string serviceName)
    {
        return GetCartableItemQuery(serviceName: serviceName).ToList();
    }
}