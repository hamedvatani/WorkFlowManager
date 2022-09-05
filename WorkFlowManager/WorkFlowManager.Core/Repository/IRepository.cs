using WorkFlowManager.Shared.Models;

namespace WorkFlowManager.Core.Repository;

public interface IRepository
{
    List<WorkFlow> GetWorkFlows(int id = 0, string name = "");
    WorkFlow AddWorkFlow(string name);

    Step AddStep(WorkFlow workFlow, string name, StepTypeEnum stepType, ProcessTypeEnum processType, string description,
        string customUser, string customRole, string addOnWorkerDllFileName, string addOnWorkerClassName);

    Step? GetStepById(int id);
    Step? GetNextStep(int stepId, string result);
    Flow AddFlow(Step sourceStep, Step destinationStep, string condition);
    Entity AddEntity(string json, string starterUser, string starterRole);
    void ChangeEntityStatus(Entity entity, EntityStatusEnum status);
    Entity? GetEntityById(int id);
    EntityLog AddEntityLog(Entity entity, Step step, EntityLogStatusEnum logType, string description);
    CartableItem AddCartableItem(Entity entity, Step step, string user, string role, string serviceName, string possibleActions);
    bool DeleteCartableItem(int id);
    CartableItem? GetCartableItemById(int id);
    List<CartableItem> GetUserCartable(string user);
    List<CartableItem> GetRoleCartable(string role);
    List<CartableItem> GetServiceCartable(string serviceName);
}