namespace JobHandler.Sender;

public abstract class SenderConfiguration
{
    public bool Durable { get; set; } = false;
    public string GroupName { get; set; } = "";
}