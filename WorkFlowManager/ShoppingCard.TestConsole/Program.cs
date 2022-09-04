using Newtonsoft.Json;
using ShoppingCard;
using WorkFlowManager.Client;
using WorkFlowManager.Shared.Models.Dto;

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

// Console.WriteLine("Press <ENTER> to start workflow ...");
// Console.ReadLine();
// string er;
// var entityId1 = biz.StartWorkFlow(JsonConvert.SerializeObject(allExistsCard), "User1", "Role1", workFlowId, out er);
// Console.WriteLine(entityId1 > 0 ? $"EntityId : {entityId1}" : $"Error : {er}");
// var entityId2 = biz.StartWorkFlow(JsonConvert.SerializeObject(noneExistsCard), "User1", "Role1", workFlowId, out er);
// Console.WriteLine(entityId2 > 0 ? $"EntityId : {entityId2}" : $"Error : {er}");
// var entityId3 = biz.StartWorkFlow(JsonConvert.SerializeObject(someExistsCard), "User1", "Role1", workFlowId, out er);
// Console.WriteLine(entityId3 > 0 ? $"EntityId : {entityId3}" : $"Error : {er}");

Console.WriteLine("Press <ENTER> to set cartable result ...");
Console.ReadLine();
var res = biz.GetUserCartable("User1");
if (res.IsSuccess)
{
    Console.WriteLine($"User1 Cartable has {res.GetResult().Count} items");
    if (res.GetResult().Count > 0)
    {
        Console.WriteLine($"Set Result for Entity {res.GetResult()[0].EntityId} to Accept");
        var r1 = biz.SetCartableItemResult(new SetCartableItemResultDto
            {CartableItemId = res.GetResult()[0].Id, Result = "Accept"});
        Console.WriteLine(r1.IsSuccess ? "Success" : r1.Message);
    }
}
else
    Console.WriteLine(res.Message);

Console.WriteLine("Press <ENTER> to exit ...");
Console.ReadLine();