namespace RabbitMq.JobHandler.Sender;

public interface IJobSender
{
    Task SendJobAsync<T>(T job);

}