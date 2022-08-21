using ShoppingCard;
using WorkFlowManager.Client;

var config = new ClientConfiguration
{
    ApiAddress = "localhost",
    ApiPort = 42578
};
var _biz = new ShoppingCardBiz(new Client(config, new ApiClient(config), new RpcClient(config)));
_biz.CreateWorkFlow();

Console.WriteLine("Press <ENTER> to exit ...");
Console.ReadLine();