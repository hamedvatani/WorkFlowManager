using WorkFlowManager.Client;
using WorkFlowManager.Client.Models;
using WorkFlowManager.Client.Models.Dto;

namespace ShoppingCard;

public class ShoppingCardBiz
{
    private readonly Client _client;

    public ShoppingCardBiz(Client client)
    {
        _client = client;
    }

    public void CreateWorkFlow()
    {
        var result = _client.GetWorkFlows(new GetWorkFlowsDto {Name = "MyWorkFlow"});
        if (!result.IsSuccess)
            return;
        if (result.ReturnValue == null)
            return;
        if (result.ReturnValue.Count == 1)
            return;

        var workFlow = _client.AddWorkFlow(new AddWorkFlowDto {Name = "MyWorkFlow", EntityName = "ShoppingCard"})
            .GetResult();

        var startStep = _client.AddStep(new AddStepDto
        {
            WorkFlowId = workFlow.Id,
            Name = "Start",
            StepType = StepTypeEnum.Start,
            ProcessType = ProcessTypeEnum.None,
            Description = "Start Step"
        }).GetResult();
        var isExistsStep = _client.AddStep(new AddStepDto
        {
            WorkFlowId = workFlow.Id,
            Name = "IsExists",
            StepType = StepTypeEnum.Condition,
            ProcessType = ProcessTypeEnum.AddOnWorker,
            Description = "Check if all items exists"
        }).GetResult();
        var doShoppingStep = _client.AddStep(new AddStepDto
        {
            WorkFlowId = workFlow.Id,
            Name = "DoShopping",
            StepType = StepTypeEnum.Process,
            ProcessType = ProcessTypeEnum.Service,
            Description = "Do Shopping"
        }).GetResult();
        var errorReportStep = _client.AddStep(new AddStepDto
        {
            WorkFlowId = workFlow.Id,
            Name = "ErrorReport",
            StepType = StepTypeEnum.Process,
            ProcessType = ProcessTypeEnum.Service,
            Description = "Report Error"
        }).GetResult();
        var getAcceptanceStep = _client.AddStep(new AddStepDto
        {
            WorkFlowId = workFlow.Id,
            Name = "GetAcceptance",
            StepType = StepTypeEnum.Condition,
            ProcessType = ProcessTypeEnum.StarterUser,
            Description = "Report Error"
        }).GetResult();
        var endStep = _client.AddStep(new AddStepDto
        {
            WorkFlowId = workFlow.Id,
            Name = "End",
            StepType = StepTypeEnum.End,
            ProcessType = ProcessTypeEnum.None,
            Description = "End Step"
        }).GetResult();

        _client.AddFlow(new AddFlowDto
        {
            SourceStepId = startStep.Id,
            DestinationStepId = isExistsStep.Id
        });
        _client.AddFlow(new AddFlowDto
        {
            SourceStepId = isExistsStep.Id,
            DestinationStepId = doShoppingStep.Id,
            Condition = "Yes"
        });
        _client.AddFlow(new AddFlowDto
        {
            SourceStepId = isExistsStep.Id,
            DestinationStepId = errorReportStep.Id,
            Condition = "No"
        });
        _client.AddFlow(new AddFlowDto
        {
            SourceStepId = isExistsStep.Id,
            DestinationStepId = getAcceptanceStep.Id,
            Condition = "Semi"
        });
        _client.AddFlow(new AddFlowDto
        {
            SourceStepId = doShoppingStep.Id,
            DestinationStepId = endStep.Id
        });
        _client.AddFlow(new AddFlowDto
        {
            SourceStepId = errorReportStep.Id,
            DestinationStepId = endStep.Id
        });
        _client.AddFlow(new AddFlowDto
        {
            SourceStepId = getAcceptanceStep.Id,
            DestinationStepId = doShoppingStep.Id,
            Condition = "Accept"
        });
        _client.AddFlow(new AddFlowDto
        {
            SourceStepId = getAcceptanceStep.Id,
            DestinationStepId = errorReportStep.Id,
            Condition = "Reject"
        });
    }
}