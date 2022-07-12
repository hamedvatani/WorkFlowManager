using WorkFlowManager.Core.Models;

namespace WorkFlowManager.Core.Interfaces;

public interface IWfManager
{
    List<WorkFlow> GetAllWorkFlows();
    WorkFlow? GetWorkFlow(string name);
    WorkFlow CreateOrGetWorkFlow(string name);
}