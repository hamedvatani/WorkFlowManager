using WorkFlowManager.Client;
using WorkFlowManager.Client.Models;

namespace ShoppingCard.Workers;

public class ErrorReport : IWorker
{
    public Task<string> RunWorkerAsync(Entity entity)
    {
        return Task.FromResult("");
    }
}