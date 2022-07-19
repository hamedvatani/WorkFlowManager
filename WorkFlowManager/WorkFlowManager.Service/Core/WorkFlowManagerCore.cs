using WorkFlowManager.Service.Models;
using WorkFlowManager.Service.Models.Dto;
using WorkFlowManager.Service.Repositories;

namespace WorkFlowManager.Service.Core;

public class WorkFlowManagerCore : IWorkFlowManagerCore
{
    private readonly IWorkFlowRepository _repository;

    public WorkFlowManagerCore(IWorkFlowRepository repository)
    {
        _repository = repository;
    }

    public MethodResult StartWorkFlow(Entity entity, WorkFlow workFlow)
    {
        var startStep = workFlow.Steps.FirstOrDefault(s => s.StepType == StepTypeEnum.Start);
        if (startStep == null)
            return MethodResult.Error("Invalid workflow!");
    }

    public MethodResult RunStep(Step step, Entity entity)
    {
        
    }
}