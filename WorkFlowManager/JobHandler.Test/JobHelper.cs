using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using JobHandler.Executor;
using JobHandler.RabbitMq.Executor;
using JobHandler.RabbitMq.Sender;
using JobHandler.Sender;
using RabbitMQ.Client;

namespace JobHandler.Test;

public class JobHelper
{
    public ConcurrentBag<Job> ReceivedJobs { get; set; } = new();

    private readonly ISender _sender;
    private readonly IExecutor<Job> _executor;

    public JobHelper(string groupName, ushort maxThreads = 1, int timeout = 10000, int maxRetries = 3)
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDelete(groupName);
            channel.QueueDelete(groupName + "_FailJobs");
            channel.ExchangeDelete(groupName + "_RetryJobs");
        }

        _sender = new RabbitMqSender(c =>
        {
            c.GroupName = groupName;
            c.Timeout = timeout;
            c.MaxRetries = maxRetries;
        });
        _executor = new RabbitMqExecutor<Job>(c =>
        {
            c.GroupName = groupName;
            c.MaxThreads = maxThreads;
        });
    }

    public void Send(Job job)
    {
        job.PublishTime = DateTime.Now;
        _sender.Send(job);
    }

    public void Send(string job)
    {
        _sender.Send(job);
    }

    public void Start(int jobDelay = 500, bool returnValue=true)
    {
        _executor.StartExecution((job, token) =>
        {
            job.ConsumeTime = DateTime.Now;
            Thread.Sleep(jobDelay);
            job.ExecuteTime = DateTime.Now;
            if (!token.IsCancellationRequested)
                ReceivedJobs.Add(job);
            return returnValue;
        });
    }

    public void Stop()
    {
        _executor.StopExecution();
    }
}