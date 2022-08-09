namespace JobHandler.Executor;

public interface IExecutor<T>
{
    void StartExecution(Func<T, CancellationToken, bool> executor);
    void StopExecution();
}