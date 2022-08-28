using WorkFlowManager.Client.Models;

namespace WorkFlowManager.Core.Repository;

public interface IRepository
{
    Task<List<WorkFlow>> GetWorkFlowsAsync(int id = 0, string name = "");
    Task<WorkFlow> AddWorkFlowAsync(string name);

    Task<Step> AddStepAsync(WorkFlow workFlow, string name, StepTypeEnum stepType, ProcessTypeEnum processType,
        string description, string customUser, string customRole, string addOnWorkerDllFileName,
        string addOnWorkerClassName);

    Task<Step?> GetStepByIdAsync(int id);
    Task<Flow> AddFlowAsync(Step sourceStep, Step destinationStep, string condition);
    Task<Entity> AddEntityAsync(string json, string starterUser, string starterRole, EntityStatusEnum status);
    Task<Entity?> GetEntityByIdAsync(int id);
    Task ChangeStatusAsync(Entity entity, Step step, EntityStatusEnum status, string description);

    Task<EntityLog> AddEntityLogAsync(Entity entity, Step? step, EntityLogTypeEnum logType, string subject,
        string description);

    Task<UserRoleCartable> AddUserRoleCartableAsync(Entity entity, Step step, string user, string role, string possibleActions);
    Task<bool> DeleteUserRoleCartableAsync(int id);
    Task<UserRoleCartable?> GetUserRoleCartableByIdAsync(int id);
    Task<List<UserRoleCartable>> GetUserCartablesAsync(string user);
    Task<List<UserRoleCartable>> GetRoleCartablesAsync(string role);

    Task<ServiceCartable> AddServiceCartableAsync(Entity entity, Step step, string serviceName, string possibleActions);
    Task<bool> DeleteServiceCartableAsync(int id);
    Task<ServiceCartable?> GetServiceCartableByIdAsync(int id);
    Task<List<ServiceCartable>> GetServiceCartablesAsync(string serviceName);
}