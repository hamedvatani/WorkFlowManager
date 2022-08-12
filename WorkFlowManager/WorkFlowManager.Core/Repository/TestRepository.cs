using WorkFlowManager.Core.Data;
using WorkFlowManager.Core.Models;

namespace WorkFlowManager.Core.Repository;

public class TestRepository : IRepository
{
    public WorkFlow? GetWorkFlow(string name)
    {
        return null;
    }

    public WorkFlow AddWorkFlow(string name, string entityName)
    {
        var wf = new WorkFlow
        {
            Id = 1,
            Name = name,
            EntityName = entityName,
            Steps = new List<Step>()
        };
        return wf;
    }
}