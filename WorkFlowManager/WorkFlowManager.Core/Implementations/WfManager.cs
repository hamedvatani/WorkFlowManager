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

    public WorkFlow CreateOrGetWorkFlow(string name)
    {
        var retVal = GetWorkFlow(name);
        if (retVal != null)
            return retVal;
        retVal = new WorkFlow {Name = name};
        _context.WorkFlows.Add(retVal);
        _context.SaveChanges();
        return retVal;
    }
}