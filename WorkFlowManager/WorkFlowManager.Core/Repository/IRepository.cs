using WorkFlowManager.Shared.Models;

namespace WorkFlowManager.Core.Repository;

public interface IRepository
{
    List<WorkFlow> GetWorkFlows(int id = 0, string name = "");
    WorkFlow AddWorkFlow(string name);

    Step AddStep(WorkFlow workFlow, string name, StepTypeEnum stepType, ProcessTypeEnum processType, string description,
        string customUser, string customRole, string addOnWorkerDllFileName, string addOnWorkerClassName);

    Step? GetStepById(int id);
    Flow AddFlow(Step sourceStep, Step destinationStep, string condition);
    Entity AddEntity(string json, string starterUser, string starterRole, EntityStatusEnum status);
    Entity? GetEntityById(int id);
    void ChangeStatus(Entity entity, Step step, EntityStatusEnum status, string description);
    EntityLog AddEntityLog(Entity entity, Step? step, EntityLogTypeEnum logType, string subject, string description);
    UserRoleCartable AddUserRoleCartable(Entity entity, Step step, string user, string role, string possibleActions);
    bool DeleteUserRoleCartable(int id);
    UserRoleCartable? GetUserRoleCartableById(int id);
    List<UserRoleCartable> GetUserCartables(string user);
    List<UserRoleCartable> GetRoleCartables(string role);
    ServiceCartable AddServiceCartable(Entity entity, Step step, string serviceName, string possibleActions);
    bool DeleteServiceCartable(int id);
    ServiceCartable? GetServiceCartableById(int id);
    List<ServiceCartable> GetServiceCartables(string serviceName);
}