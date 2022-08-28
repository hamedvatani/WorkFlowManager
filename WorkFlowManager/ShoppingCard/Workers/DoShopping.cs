using WorkFlowManager.Client;
using WorkFlowManager.Client.Models;

namespace ShoppingCard.Workers;

public class DoShopping : IWorker
{
    public Task<string> RunWorkerAsync(Entity entity)
    {
        return Task.FromResult("");
    }
}