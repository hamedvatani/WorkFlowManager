using WorkFlowManager.Core.Models;

namespace WorkFlowManager.Core.Interfaces;

public interface IWfManager
{
    WorkFlow? GetWorkFlow(string name);
}