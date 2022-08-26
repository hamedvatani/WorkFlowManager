using ShoppingCard;
using WorkFlowManager.Client;

var config = new ClientConfiguration
{
    ApiAddress = "localhost",
    ApiPort = 42578
};
var biz = new ShoppingCardBiz(new Client(config, new ApiClient(config)));
int workFlowId = biz.CreateWorkFlow();
var allExistsCard = biz.CreateAllExistsCard();

Console.WriteLine("Press <ENTER> to exit ...");
Console.ReadLine();