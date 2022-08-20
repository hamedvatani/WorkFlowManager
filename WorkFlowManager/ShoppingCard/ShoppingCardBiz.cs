using WorkFlowManager.Client;

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
       var result = _client.GetWorkFlows(name: "MyWorkFlow");
       if (!result.IsSuccess)
           return;
       if (result.ReturnValue == null)
           return;
       if (result.ReturnValue.Count == 1)
           return;

    }
}