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
        _client.StartAsync(CancellationToken.None);
    }

    public int CreateWorkFlow()
    {
        var result = _client.GetWorkFlows(new GetWorkFlowsDto {Name = "MyWorkFlow"});
        if (!result.IsSuccess)
            return 0;
        if (result.ReturnValue == null)
            return 0;
        if (result.ReturnValue.Count == 1)
            return result.GetResult()[0].Id;

        var workFlow = _client.AddWorkFlow(new AddWorkFlowDto {Name = "MyWorkFlow"})
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

        return workFlow.Id;
    }

    public Card CreateAllExistsCard()
    {
        return new Card
        {
            Items = new List<Item>
            {
                new Item
                {
                    Name = "Shoe",
                    Quantity = 10,
                    Stock = 20
                },
                new Item
                {
                    Name = "TShirt",
                    Quantity = 3,
                    Stock = 15
                },
                new Item
                {
                    Name = "Sunglasses",
                    Quantity = 2,
                    Stock = 10
                },
                new Item
                {
                    Name = "Watch",
                    Quantity = 15,
                    Stock = 30
                }
            }
        };
    }

    public MethodResult<int> StartWorkFlow(Card card, string user, int workFlowId)
    {
        return _client.StartWorkFlow(card, user, "", workFlowId);
    }
}