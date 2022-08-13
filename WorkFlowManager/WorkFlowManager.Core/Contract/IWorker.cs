namespace WorkFlowManager.Core.Contract;

public interface IWorker
{
    MethodResult Process(IEntity entity, List<MethodResult> resultLogs);
}