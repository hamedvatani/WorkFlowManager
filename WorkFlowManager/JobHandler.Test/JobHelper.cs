using System;
using System.Collections.Concurrent;
using System.Threading;
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

    public JobHelper(string groupName, ushort maxThreads = 1)
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
        }

        _sender = new RabbitMqSender(c => c.GroupName = groupName);
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

    public void Start()
    {
        _executor.StartExecution(job =>
        {
            job.ConsumeTime = DateTime.Now;
            Thread.Sleep(500);
            job.ExecuteTime = DateTime.Now;
            ReceivedJobs.Add(job);
            return true;
        });
    }

    public void Stop()
    {
        _executor.StopExecution();
    }
}