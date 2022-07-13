using WorkFlowManager.Core.Context;
using WorkFlowManager.Core.Interfaces;
using WorkFlowManager.Core.Models;

namespace WorkFlowManager.Core.Implementations;

public class WfManager:IWfManager
{
    private readonly WorkFlowManagerContext _context;

    public WfManager(WorkFlowManagerContext context)
    {
        _context = context;
        context.Database.EnsureCreated();
    }

    public List<WorkFlow> GetAllWorkFlows()
    {
        return _context.WorkFlows.ToList();
    }

    public WorkFlow? GetWorkFlow(string name)
    {
        return _context.WorkFlows.FirstOrDefault(w => w.Name == name);
    }

    public WorkFlow CreateWorkFlow(string name)
    {
        if (_context.WorkFlows.FirstOrDefault(w => w.Name == name) != null)
            throw new Exception($"{name} already exists!");
        var retVal = new WorkFlow {Name = name};
        _context.WorkFlows.Add(retVal);
        _context.SaveChanges();
        return retVal;
    }

    public void DeleteWorkFlow(string name)
    {
        var wf = _context.WorkFlows.FirstOrDefault(w => w.Name == name);
        if (wf != null)
            _context.WorkFlows.Remove(wf);
        _context.SaveChanges();
    }

    public Step CreateStep(WorkFlow parent, StepTypeEnum stepType, string name, string description)
    {
        if (_context.Steps.FirstOrDefault(s => s.Name == name && s.WorkFlowId == parent.Id) != null)
            throw new Exception($"{name} already exists!");
        var retVal = new Step
        {
            WorkFlow = parent,
            StepType = stepType,
            Name = name,
            Description = description
        };
        _context.Steps.Add(retVal);
        _context.SaveChanges();
        return retVal;
    }

    public Flow CreateFlow(Step sourceStep, Step destinationStep, string condition)
    {
        var retVal = new Flow
        {
            SourceStep = sourceStep,
            DestinationStep = destinationStep,
            Condition = condition
        };
        _context.Flows.Add(retVal);
        _context.SaveChanges();
        return retVal;
    }
}