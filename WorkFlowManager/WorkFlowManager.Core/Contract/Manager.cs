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
}