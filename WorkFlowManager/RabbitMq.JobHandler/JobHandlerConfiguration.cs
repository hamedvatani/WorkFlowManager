namespace RabbitMq.JobHandler;

public class JobHandlerConfiguration
{
    public string HostName { get; set; } = "localhost";
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public int MaxThreads { get; set; } = 1;
    public string GroupName { get; set; } = "";
    public bool Durable { get; set; } = false;
}