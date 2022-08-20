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
       var result = _client.GetWorkFlows(new GetWorkFlowsDto{Name = "MyWorkFlow"});
       if (!result.IsSuccess)
           return;
       if (result.ReturnValue == null)
           return;
       if (result.ReturnValue.Count == 1)
           return;

       var workFlow = _client.AddWorkFlow(new AddWorkFlowDto { Name = "MyWorkFlow", EntityName = "ShoppingCard" }).GetResult();

       var isExistsStep = _client.AddStep(new AddStepDto
       {
           WorkFlowId = workFlow.Id,
           Name = "IsExists",
           StepType = StepTypeEnum.Condition,
           ProcessType = ProcessTypeEnum.Service,
           Description = "Check if all items exists"
       }).GetResult();
    }
}