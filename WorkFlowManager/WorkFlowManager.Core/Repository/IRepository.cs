using WorkFlowManager.Client.Models;

namespace WorkFlowManager.Core.Repository;

public interface IRepository
{
    List<WorkFlow> GetWorkFlows(int id = 0, string name = "");
    WorkFlow AddWorkFlow(string name, string entityName);

    Step AddStep(WorkFlow workFlow, string name, StepTypeEnum stepType, ProcessTypeEnum processType, string description,
        string customUser, string customRole);

    Step? GetStepById(int stepId);
    Flow AddFlow(Step sourceStep, Step destinationStep, string condition);
}