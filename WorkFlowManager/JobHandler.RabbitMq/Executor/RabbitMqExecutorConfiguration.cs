using JobHandler.Executor;

namespace JobHandler.RabbitMq.Executor;

public class RabbitMqExecutorConfiguration : ExecutorConfiguration
{
    public string HostName { get; set; } = "";
    public string UserName { get; set; } = "";
    public string Password { get; set; } = "";
}