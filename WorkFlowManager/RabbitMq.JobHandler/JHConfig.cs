namespace RabbitMq.JobHandler;

public class JHConfig
{
    public string HostName { get; set; } = "";
    public string UserName { get; set; } = "";
    public string Password { get; set; } = "";
    public int MaxThreads { get; set; } = 1;
    public string QueueName { get; set; } = "";
}