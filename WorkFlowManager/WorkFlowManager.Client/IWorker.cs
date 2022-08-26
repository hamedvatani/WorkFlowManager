using WorkFlowManager.Client.Models;

namespace WorkFlowManager.Client;

public interface IWorker
{
    Task<string> RunWorkerAsync(Entity entity);
}