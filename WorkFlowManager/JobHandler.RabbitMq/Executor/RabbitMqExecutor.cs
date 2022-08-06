using System.Text;
using JobHandler.Executor;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace JobHandler.RabbitMq.Executor;

public class RabbitMqExecutor<T> : IExecutor<T>
{
    private readonly RabbitMqExecutorConfiguration _configuration;
    private ConnectionFactory _factory = null!;
    private IConnection _connection = null!;
    private IModel _channel = null!;
    private EventingBasicConsumer _consumer = null!;
    private string _consumerTag = "";
    private string _failJobsQueue => _configuration.GroupName + "_FailJobs";

    public RabbitMqExecutor(RabbitMqExecutorConfiguration configuration)
    {
        _configuration = configuration;
        Setup();
    }

    public RabbitMqExecutor(Action<RabbitMqExecutorConfiguration> configBuilder)
    {
        _configuration = new RabbitMqExecutorConfiguration();
        configBuilder(_configuration);
        Setup();
    }

    private void Setup()
    {
        _factory = new ConnectionFactory
        {
            HostName = _configuration.HostName,
            UserName = _configuration.UserName,
            Password = _configuration.Password
        };
        _connection = _factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(_configuration.GroupName, _configuration.Durable, false, false);
        _channel.QueueDeclare(_failJobsQueue, true, false, false);
        _channel.BasicQos(0, _configuration.MaxThreads, false);
        _consumer = new EventingBasicConsumer(_channel);
    }

    public void StartExecution(Func<T, bool> executor)
    {
        _consumer.Received += (sender, ea) =>
        {
            Task.Run(() =>
            {
                var bytes = ea.Body.ToArray();
                var str = Encoding.UTF8.GetString(bytes);
                T? job = default(T);
                try
                {
                    job = JsonConvert.DeserializeObject<T>(str);
                    if (job != null && executor(job))
                        _channel.BasicAck(ea.DeliveryTag, false);
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                    var props = _channel.CreateBasicProperties();
                    props.Persistent = true;
                    props.Headers = new Dictionary<string, object>();
                    props.Headers.Add("Error", ex.Message);
                    _channel.BasicPublish("", _failJobsQueue, props, ea.Body);
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
            });
        };
        _consumerTag = _channel.BasicConsume(_configuration.GroupName, false, _consumer);
    }

    public void StopExecution()
    {
        if (!string.IsNullOrEmpty(_consumerTag))
            _channel.BasicCancel(_consumerTag);
    }

}