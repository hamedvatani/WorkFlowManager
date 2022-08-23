namespace WorkFlowManager.Core;

public class ManagerConfiguration
{
    public string RabbitMqHostName { get; set; } = "127.0.0.1";
    public string RabbitMqUserName { get; set; } = "guest";
    public string RabbitMqPassword { get; set; } = "guest";
    public string QueueName { get; set; } = "WorkFlowManager";
    public int Timeout { get; set; } = 30000;
}