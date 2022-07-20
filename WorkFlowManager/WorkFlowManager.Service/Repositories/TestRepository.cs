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
                StarterUser = "TestUser",
                StarterRole = "",
                Steps = new List<Step>()
            };

        var startStep = new Step
        {
            Id = 1,
            StepType = StepTypeEnum.Start,
            Name = "Start",
            Description = "Start Step",
            ProcessType = ProcessTypeEnum.None,
            User = "",
            Role = "",
            WorkFlowId = 1,
            WorkFlow = _wf,
            AddOneWorkerId = null,
            AddOneWorker = null,
            Heads = new List<Flow>(),
            Tails = new List<Flow>()
        };
        _wf.Steps.Add(startStep);
    }

    public WorkFlow? GetWorkFlow(string workFlowName)
    {
        return workFlowName == "1" ? _wf : null;
    }

    public MethodResult AddEntityFlow(Entity entity, Step step)
    {
        return MethodResult.Success;
    }
}