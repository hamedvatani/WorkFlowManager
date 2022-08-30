using Newtonsoft.Json;
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
var noneExistsCard = biz.CreateNoneExistsCard();
var someExistsCard = biz.CreateSomeExistsCard();

Console.WriteLine("Press <ENTER> to start workflow ...");
Console.ReadLine();
string er;
var entityId = biz.StartWorkFlow(JsonConvert.SerializeObject(allExistsCard), "User1", "Role1", workFlowId, out er);
Console.WriteLine(entityId > 0 ? $"EntityId : {entityId}" : $"Error : {er}");

Console.WriteLine("Press <ENTER> to exit ...");
Console.ReadLine();