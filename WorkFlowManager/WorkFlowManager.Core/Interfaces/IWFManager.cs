using WorkFlowManager.Core.Models;

namespace WorkFlowManager.Core.Interfaces;

public interface IWfManager
{
    List<WorkFlow> GetAllWorkFlows();
    WorkFlow? GetWorkFlow(string name);
    WorkFlow CreateWorkFlow(string name);
    void DeleteWorkFlow(string name);
    Step CreateStep(WorkFlow parent, StepTypeEnum stepType, string name, string description);
    Flow CreateFlow(Step sourceStep, Step destinationStep, string condition);
}