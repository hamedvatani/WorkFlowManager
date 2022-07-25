using Microsoft.AspNetCore.Mvc;
using WorkFlowManager.Service.Models;
using WorkFlowManager.Service.Models.Dto;

namespace WorkFlowManager.Service.Core;

public static class Extensions
{
    public static MethodResult Validate(this WorkFlow workFlow)
    {
        if (workFlow.Steps.Count(step => step.StepType == StepTypeEnum.Start) != 1)
            return MethodResult.Error("Workflow has to have exact one start step");
        var start = workFlow.Steps.FirstOrDefault(step => step.StepType == StepTypeEnum.Start);
        if (start == null)
            return MethodResult.Error("");
        if (start.Tails.Count > 0)
            return MethodResult.Error("There is a flow to start step");
        if (start.ProcessType != ProcessTypeEnum.None)
            return MethodResult.Error("Start step process type must be None");

        if (workFlow.Steps.Count(step => step.StepType == StepTypeEnum.End) != 1)
            return MethodResult.Error("Workflow has to have exact one end step");
        var end = workFlow.Steps.FirstOrDefault(step => step.StepType == StepTypeEnum.End);
        if (end == null)
            return MethodResult.Error("");
        if (end.Heads.Count > 0)
            return MethodResult.Error("There is a flow from end step");
        if (end.ProcessType != ProcessTypeEnum.None)
            return MethodResult.Error("End step process type must be None");

        foreach (var step in workFlow.Steps)
        {
            if (step.Heads.Count(flow => flow.Condition == "") > 1)
                return MethodResult.Error($"Step {step.Name} has more than one unconditional flow");
            if (step.StepType != StepTypeEnum.Start && step.Tails.Count == 0)
                return MethodResult.Error($"There is no flow to step {step.Name}");
            if (step.StepType != StepTypeEnum.End && step.Heads.Count == 0)
                return MethodResult.Error($"There is no flow from step {step.Name}");
        }

        return MethodResult.Success;
    }
}