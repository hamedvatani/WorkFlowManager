namespace RPC.RabbitMq;

public class RpcConfiguration
{
    public string RabbitMqHostName { get; set; } = "localhost";
    public string RabbitMqUserName { get; set; } = "guest";
    public string RabbitMqPassword { get; set; } = "guest";
    public string InputQueueName { get; set; } = "RpcQueueName.Input";
    public string OutputQueueName { get; set; } = "RpcQueueName.Output";
    public int Timeout { get; set; } = 30000;
}