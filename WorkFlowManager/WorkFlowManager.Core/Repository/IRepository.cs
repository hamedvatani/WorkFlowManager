using WorkFlowManager.Client.Models;

namespace WorkFlowManager.Core.Repository;

public interface IRepository
{
    Task<List<WorkFlow>> GetWorkFlowsAsync(int id = 0, string name = "");
    Task<WorkFlow> AddWorkFlowAsync(string name);

    Task<Step> AddStepAsync(WorkFlow workFlow, string name, StepTypeEnum stepType, ProcessTypeEnum processType,
        string description,
        string customUser, string customRole);

    Task<Step?> GetStepByIdAsync(int id);
    Task<Flow> AddFlowAsync(Step sourceStep, Step destinationStep, string condition);
    Task<Entity> AddEntityAsync(string json, string starterUser, string starterRole, EntityStatusEnum status);
    Task ChangeStatusAsync(Entity entity, Step step, EntityStatusEnum status);

    Task<EntityLog> AddEntityLogAsync(Entity entity, Step? step, EntityLogTypeEnum logType, string subject,
        string description);
}