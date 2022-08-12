using WorkFlowManager.Core.Models;

namespace WorkFlowManager.Core.Repository;

public interface IRepository
{
    WorkFlow? GetWorkFlow(string name);
    WorkFlow AddWorkFlow(string name, string entityName);
}