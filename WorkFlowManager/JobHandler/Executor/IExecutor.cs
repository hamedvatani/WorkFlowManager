namespace JobHandler.Executor;

public interface IExecutor<T>
{
    void StartExecution(Func<T, bool> executor);
    void StopExecution();
}