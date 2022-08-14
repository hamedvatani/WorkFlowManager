﻿using WorkFlowManager.Core.Models;

namespace WorkFlowManager.Core.Contract;

public interface IManager
{
    AddOnWorker AddWorker(string fileName, string className);
    WorkFlow? GetWorkFlow(string name);
    WorkFlow AddWorkFlow(string name, string entityName);
    Step AddStep(WorkFlow workFlow, string name, StepTypeEnum stepType, ProcessTypeEnum processType, string description,
        string customUser, string customRole, AddOnWorker? addOnWorker);

    Flow AddFlow(Step sourceStep, Step destinationStep, string condition);
    MethodResult ValidateWorkFlow(WorkFlow workFlow);
}