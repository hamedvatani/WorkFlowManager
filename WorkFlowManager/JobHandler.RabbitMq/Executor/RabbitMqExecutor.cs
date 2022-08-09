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
    private string FailJobsQueue => _configuration.GroupName + "_FailJobs";
    private string RetryJobsExchange => _configuration.GroupName + "_RetryJobs";

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
        _channel.QueueDeclare(FailJobsQueue, true, false, false);
        _channel.ExchangeDeclare(RetryJobsExchange, "x-delayed-message", true, false,
            new Dictionary<string, object> {{"x-delayed-type", "direct"}});
        _channel.QueueBind(queue: _configuration.GroupName, exchange: RetryJobsExchange, routingKey: "");
        _channel.BasicQos(0, _configuration.MaxThreads, false);
        _consumer = new EventingBasicConsumer(_channel);
    }

    public void StartExecution(Func<T, CancellationToken, bool> executor)
    {
        _consumer.Received += async (_, ea) =>
        {
            var bytes = ea.Body.ToArray();
            var str = Encoding.UTF8.GetString(bytes);
            try
            {
                var job = JsonConvert.DeserializeObject<T>(str);
                if (job == null)
                    FailJob(ea, "Cant parse job");
                else
                {
                    var timeout = (int) ea.BasicProperties.Headers["Timeout"];
                    var retries = (int) ea.BasicProperties.Headers["Retries"];
                    var maxRetries = (int) ea.BasicProperties.Headers["MaxRetries"];
                    var source = new CancellationTokenSource(timeout);
                    var task = Task.Run(() => executor(job, source.Token), source.Token);
                    var t = await Task.WhenAny(task, Task.Delay(timeout, source.Token));
                    retries++;
                    ea.BasicProperties.Headers["Retries"] = retries;
                    if (task == t)
                    {
                        if (!task.Result)
                        {
                            if (retries >= maxRetries)
                                FailJob(ea, "Job fails!");
                            else
                                RetryJob(ea, "Job fails!");
                        }
                    }
                    else
                    {
                        if (retries >= maxRetries)
                            FailJob(ea, "Job timeout!");
                        else
                            RetryJob(ea, "Job timeout!");
                    }
                }
            }
            catch (Exception ex)
            {
                FailJob(ea, ex.Message);
            }
            finally
            {
                _channel.BasicAck(ea.DeliveryTag, false);
            }
        };
        _consumerTag = _channel.BasicConsume(_configuration.GroupName, false, _consumer);
    }

    public void StopExecution()
    {
        if (!string.IsNullOrEmpty(_consumerTag))
            _channel.BasicCancel(_consumerTag);
    }

    private void FailJob(BasicDeliverEventArgs ea, string message)
    {
        var props = ea.BasicProperties;
        props.Persistent = true;
        props.Headers ??= new Dictionary<string, object>();
        props.Headers.Add("Error", message);
        _channel.BasicPublish("", FailJobsQueue, props, ea.Body);
    }

    private void RetryJob(BasicDeliverEventArgs ea, string message)
    {
        var props = ea.BasicProperties;
        if (props.Headers.ContainsKey("x-delay"))
            props.Headers["x-delay"] = _configuration.RetryDelay;
        else
            props.Headers.Add("x-delay", _configuration.RetryDelay);
        props.Headers.Add("Error " + props.Headers["Retries"], message);
        _channel.BasicPublish(RetryJobsExchange, "", props, ea.Body);
    }
}