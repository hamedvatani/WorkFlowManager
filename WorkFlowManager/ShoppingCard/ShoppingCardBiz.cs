using WorkFlowManager.Client;
using WorkFlowManager.Shared.Models;
using WorkFlowManager.Shared.Models.Dto;

namespace ShoppingCard;

public class ShoppingCardBiz
{
    private readonly Client _client;

    public ShoppingCardBiz(Client client)
    {
        _client = client;
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

        var startStep = _client.AddStartStep(new AddStartStepDto
        {
            WorkFlowId = workFlow.Id,
            Name = "Start",
            Description = "Start Step"
        }).GetResult();
        var isExistsStep = _client.AddAddOnWorkerStep(new AddAddOnWorkerStepDto
        {
            WorkFlowId = workFlow.Id,
            Name = "IsExists",
            StepType = StepTypeEnum.Condition,
            Description = "Check if all items exists",
            AddOnWorkerDllFileName = "ShoppingCard.dll",
            AddOnWorkerClassName = "ShoppingCard.Workers.IsExists"
        }).GetResult();
        var doShoppingStep = _client.AddAddOnWorkerStep(new AddAddOnWorkerStepDto
        {
            WorkFlowId = workFlow.Id,
            Name = "DoShopping",
            StepType = StepTypeEnum.Process,
            Description = "Do Shopping",
            AddOnWorkerDllFileName = "ShoppingCard.dll",
            AddOnWorkerClassName = "ShoppingCard.Workers.DoShopping"
        }).GetResult();
        var errorReportStep = _client.AddAddOnWorkerStep(new AddAddOnWorkerStepDto
        {
            WorkFlowId = workFlow.Id,
            Name = "ErrorReport",
            StepType = StepTypeEnum.Process,
            Description = "Report Error",
            AddOnWorkerDllFileName = "ShoppingCard.dll",
            AddOnWorkerClassName = "ShoppingCard.Workers.ErrorReport"
        }).GetResult();
        var getAcceptanceStep = _client.AddStarterUserRoleCartableStep(new AddStarterUserRoleCartableStepDto
        {
            WorkFlowId = workFlow.Id,
            Name = "GetAcceptance",
            StepType = StepTypeEnum.Condition,
            Description = "Get User Acceptance"
        }).GetResult();
        var endStep = _client.AddEndStep(new AddEndStepDto
        {
            WorkFlowId = workFlow.Id,
            Name = "End",
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

    public Card CreateNoneExistsCard()
    {
        return new Card
        {
            Items = new List<Item>
            {
                new Item
                {
                    Name = "Shoe",
                    Quantity = 30,
                    Stock = 20
                },
                new Item
                {
                    Name = "TShirt",
                    Quantity = 20,
                    Stock = 15
                },
                new Item
                {
                    Name = "Sunglasses",
                    Quantity = 15,
                    Stock = 10
                },
                new Item
                {
                    Name = "Watch",
                    Quantity = 50,
                    Stock = 30
                }
            }
        };
    }

    public Card CreateSomeExistsCard()
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
                    Quantity = 30,
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

    public int StartWorkFlow(string json, string starterUser, string starterRole, int workFlowId,out string error)
    {
        error = "";
        var result = _client.StartWorkFlow(new StartWorkFlowDto
        {
            Json = json,
            StarterUser = starterUser,
            StarterRole = starterRole,
            WorkFlowId = workFlowId
        });
        if (result.IsSuccess)
            return result.GetResult().Id;
        error = result.Message;
        return -1;
    }
}