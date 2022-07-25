using WorkFlowManager.Service.Models;
using WorkFlowManager.Service.Models.Dto;

namespace WorkFlowManager.Service.Repositories;

public class TestRepository : IWorkFlowRepository
{
    private readonly WorkFlow _wf;

    public TestRepository()
    {
        _wf = new WorkFlow
            {
                Id = 1,
                Name = "1",
                EntityName = "ShoppingCard",
                Steps = new List<Step>()
            };

        var startStep = new Step
        {
            Id = 1,
            StepType = StepTypeEnum.Start,
            Name = "Start",
            Description = "Start Step",
            ProcessType = ProcessTypeEnum.None,
            CustomUser = "",
            CustomRole = "",
            WorkFlowId = 1,
            WorkFlow = _wf,
            AddOneWorkerId = null,
            AddOneWorker = null,
            Heads = new List<Flow>(),
            Tails = new List<Flow>()
        };
        _wf.Steps.Add(startStep);

        var isExistsStep = new Step
        {
            Id = 2,
            StepType = StepTypeEnum.Condition,
            Name = "IsExists",
            Description = "Check For Items Existence",
            ProcessType = ProcessTypeEnum.AddOnWorker,
            CustomUser = "",
            CustomRole = "",
            WorkFlowId = 1,
            WorkFlow = _wf,
            AddOneWorkerId = null,
            AddOneWorker = null,
            Heads = new List<Flow>(),
            Tails = new List<Flow>()
        };
        _wf.Steps.Add(isExistsStep);

        var doShoppingStep = new Step
        {
            Id = 3,
            StepType = StepTypeEnum.Process,
            Name = "DoShopping",
            Description = "Do Shopping Process",
            ProcessType = ProcessTypeEnum.Service,
            CustomUser = "",
            CustomRole = "",
            WorkFlowId = 1,
            WorkFlow = _wf,
            AddOneWorkerId = null,
            AddOneWorker = null,
            Heads = new List<Flow>(),
            Tails = new List<Flow>()
        };
        _wf.Steps.Add(doShoppingStep);

        var errorReportStep = new Step
        {
            Id = 4,
            StepType = StepTypeEnum.Start,
            Name = "ErrorReport",
            Description = "Error Report Process",
            ProcessType = ProcessTypeEnum.Service,
            CustomUser = "",
            CustomRole = "",
            WorkFlowId = 1,
            WorkFlow = _wf,
            AddOneWorkerId = null,
            AddOneWorker = null,
            Heads = new List<Flow>(),
            Tails = new List<Flow>()
        };
        _wf.Steps.Add(errorReportStep);

        var getAcceptanceStep = new Step
        {
            Id = 5,
            StepType = StepTypeEnum.Condition,
            Name = "GetAcceptance",
            Description = "Get User Acceptance",
            ProcessType = ProcessTypeEnum.StarterUser,
            CustomUser = "",
            CustomRole = "",
            WorkFlowId = 1,
            WorkFlow = _wf,
            AddOneWorkerId = null,
            AddOneWorker = null,
            Heads = new List<Flow>(),
            Tails = new List<Flow>()
        };
        _wf.Steps.Add(getAcceptanceStep);

        var endStep = new Step
        {
            Id = 6,
            StepType = StepTypeEnum.End,
            Name = "End",
            Description = "End Step",
            ProcessType = ProcessTypeEnum.None,
            CustomUser = "",
            CustomRole = "",
            WorkFlowId = 1,
            WorkFlow = _wf,
            AddOneWorkerId = null,
            AddOneWorker = null,
            Heads = new List<Flow>(),
            Tails = new List<Flow>()
        };
        _wf.Steps.Add(endStep);

        Flow f;

        f = new Flow
        {
            Id = 1,
            Condition = "",
            SourceStepId = startStep.Id,
            SourceStep = startStep,
            DestinationStepId = isExistsStep.Id,
            DestinationStep = isExistsStep
        };
        startStep.Heads.Add(f);
        isExistsStep.Tails.Add(f);

        f = new Flow
        {
            Id = 2,
            Condition = "Yes",
            SourceStepId = isExistsStep.Id,
            SourceStep = isExistsStep,
            DestinationStepId = doShoppingStep.Id,
            DestinationStep = doShoppingStep
        };
        isExistsStep.Heads.Add(f);
        doShoppingStep.Tails.Add(f);

        f = new Flow
        {
            Id = 3,
            Condition = "No",
            SourceStepId = isExistsStep.Id,
            SourceStep = isExistsStep,
            DestinationStepId = errorReportStep.Id,
            DestinationStep = errorReportStep
        };
        isExistsStep.Heads.Add(f);
        errorReportStep.Tails.Add(f);

        f = new Flow
        {
            Id = 4,
            Condition = "Semi",
            SourceStepId = isExistsStep.Id,
            SourceStep = isExistsStep,
            DestinationStepId = getAcceptanceStep.Id,
            DestinationStep = getAcceptanceStep
        };
        isExistsStep.Heads.Add(f);
        getAcceptanceStep.Tails.Add(f);

        f = new Flow
        {
            Id = 5,
            Condition = "",
            SourceStepId = doShoppingStep.Id,
            SourceStep = doShoppingStep,
            DestinationStepId = endStep.Id,
            DestinationStep = endStep
        };
        doShoppingStep.Heads.Add(f);
        endStep.Tails.Add(f);

        f = new Flow
        {
            Id = 6,
            Condition = "",
            SourceStepId = errorReportStep.Id,
            SourceStep = errorReportStep,
            DestinationStepId = endStep.Id,
            DestinationStep = endStep
        };
        errorReportStep.Heads.Add(f);
        endStep.Tails.Add(f);

        f = new Flow
        {
            Id = 7,
            Condition = "Accept",
            SourceStepId = getAcceptanceStep.Id,
            SourceStep = getAcceptanceStep,
            DestinationStepId = doShoppingStep.Id,
            DestinationStep = doShoppingStep
        };
        getAcceptanceStep.Heads.Add(f);
        doShoppingStep.Tails.Add(f);

        f = new Flow
        {
            Id = 8,
            Condition = "Reject",
            SourceStepId = getAcceptanceStep.Id,
            SourceStep = getAcceptanceStep,
            DestinationStepId = errorReportStep.Id,
            DestinationStep = errorReportStep
        };
        getAcceptanceStep.Heads.Add(f);
        errorReportStep.Tails.Add(f);
    }

    // public WorkFlow? GetWorkFlow(string workFlowName)
    // {
    //     return workFlowName == "1" ? _wf : null;
    // }

    // public MethodResult AddEntityFlow(Entity entity, Step step)
    // {
    //     return MethodResult.Success;
    // }
}