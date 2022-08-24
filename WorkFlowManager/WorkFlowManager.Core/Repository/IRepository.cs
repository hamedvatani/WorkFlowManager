using WorkFlowManager.Client.Models;

namespace WorkFlowManager.Core.Repository;

public interface IRepository
{
    List<WorkFlow> GetWorkFlows(int id = 0, string name = "");
    WorkFlow AddWorkFlow(string name);

    Step AddStep(WorkFlow workFlow, string name, StepTypeEnum stepType, ProcessTypeEnum processType, string description,
        string customUser, string customRole);

    Step? GetStepById(int stepId);
    Flow AddFlow(Step sourceStep, Step destinationStep, string condition);
    Entity AddEntity(string json, string starterUser, string starterRole);

    EntityLog AddEntityLog(Entity entity, DateTime timeStamp, EntityLogSeverityEnum severity, string subject,
        string description);

    Entity? GetEntityById(int id);
}