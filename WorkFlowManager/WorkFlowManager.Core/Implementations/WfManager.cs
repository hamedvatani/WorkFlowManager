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

    public WorkFlow? GetWorkFlow(string name)
    {
        return _context.WorkFlows.FirstOrDefault(w => string.Equals(w.Name, name, StringComparison.CurrentCultureIgnoreCase));
    }

    public WorkFlow CreateWorkFlow(string name)
    {
        var retVal = new WorkFlow {Name = name};
        _context.WorkFlows.Add(retVal);
        _context.SaveChanges();
        return retVal;
    }

    public Step CreateStep(WorkFlowStepTypeEnum type)
    {
        throw new NotImplementedException();
    }
}