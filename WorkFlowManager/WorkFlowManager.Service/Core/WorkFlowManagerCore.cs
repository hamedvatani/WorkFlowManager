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
        return RunStep(startStep, entity);
    }

    public MethodResult RunStep(Step step, Entity entity)
    {








        return MethodResult.Success;

        // var res = _repository.AddEntityFlow(entity, step);
        // if (!res.IsSuccess)
        // {
        //     // Todo : have to create failed job
        //     return res;
        // }
        //
        // switch (step.StepType)
        // {
        //     case StepTypeEnum.Start:
        //         break;
        //     case StepTypeEnum.End:
        //         break;
        //     case StepTypeEnum.Condition:
        //         break;
        //     case StepTypeEnum.Process:
        //         break;
        //     default:
        //         return MethodResult.Error("Unknown step type!");
        // }
    }
}