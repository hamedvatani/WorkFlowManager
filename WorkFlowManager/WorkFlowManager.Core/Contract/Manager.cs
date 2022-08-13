using WorkFlowManager.Core.Models;
using WorkFlowManager.Core.Repository;

namespace WorkFlowManager.Core.Contract;

public class Manager : IManager
{
    private readonly IRepository _repository;

    public Manager(IRepository repository)
    {
        _repository = repository;
    }

    public WorkFlow? GetWorkFlow(string name)
    {
        return _repository.GetWorkFlow(name);
    }

    public WorkFlow AddWorkFlow(string name, string entityName)
    {
        return _repository.AddWorkFlow(name, entityName);
    }

    public Step AddStep(WorkFlow workFlow, string name, StepTypeEnum stepType, ProcessTypeEnum processType,
        string description, string customUser, string customRole, AddOnWorker? addOnWorker)
    {
        return _repository.AddStep(workFlow, name, stepType, processType, description, customUser, customRole,
            addOnWorker);
    }

    public Flow AddFlow(Step sourceStep, Step destinationStep, string condition)
    {
        return _repository.AddFlow(sourceStep, destinationStep, condition);
    }
}