using WorkFlowManager.Core.Models;

namespace WorkFlowManager.Core.Interfaces;

public interface IWfManager
{
    WorkFlow? GetWorkFlow(string name);
    WorkFlow CreateWorkFlow(string name);
    Step CreateStep(WorkFlowStepTypeEnum type);
}