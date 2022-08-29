using WorkFlowManager.Shared.Models;

namespace WorkFlowManager.Shared;

public interface IWorker
{
    string RunWorker(Entity entity);
}