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
var entityId1 = biz.StartWorkFlow(JsonConvert.SerializeObject(allExistsCard), "User1", "Role1", workFlowId, out er);
Console.WriteLine(entityId1 > 0 ? $"EntityId : {entityId1}" : $"Error : {er}");
var entityId2 = biz.StartWorkFlow(JsonConvert.SerializeObject(noneExistsCard), "User1", "Role1", workFlowId, out er);
Console.WriteLine(entityId2 > 0 ? $"EntityId : {entityId2}" : $"Error : {er}");
var entityId3 = biz.StartWorkFlow(JsonConvert.SerializeObject(someExistsCard), "User1", "Role1", workFlowId, out er);
Console.WriteLine(entityId3 > 0 ? $"EntityId : {entityId3}" : $"Error : {er}");

Console.WriteLine("Press <ENTER> to exit ...");
Console.ReadLine();