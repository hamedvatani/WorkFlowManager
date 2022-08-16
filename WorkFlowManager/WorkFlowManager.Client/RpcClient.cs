using System.Collections.Concurrent;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace WorkFlowManager.Client;

public class RpcClient
{
    private readonly ClientConfiguration _configuration;

    private IConnection _connection = null!;
    private IModel _channel = null!;
    private ConcurrentDictionary<string, TaskCompletionSource<RpcDto>?> _callbackMapper = new();
    
    public RpcClient(ClientConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<RpcDto> CallAsync(RpcDto message, CancellationToken cancellationToken = default(CancellationToken))
    {
        IBasicProperties props = _channel.CreateBasicProperties();
        var correlationId = Guid.NewGuid().ToString();
        props.CorrelationId = correlationId;
        var tcs = new TaskCompletionSource<RpcDto>();
        _callbackMapper.TryAdd(correlationId, tcs);

        _channel.BasicPublish(
            exchange: "",
            routingKey: _configuration.QueueName + ".Input",
            basicProperties: props,
            body: message.Serialize());

        cancellationToken.Register(() => _callbackMapper.TryRemove(correlationId, out _));
        return tcs.Task;
    }

    public void Start()
    {
        var factory = new ConnectionFactory()
        {
            HostName = _configuration.RabbitMqHostName,
            UserName = _configuration.RabbitMqUserName,
            Password = _configuration.RabbitMqPassword
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(_configuration.QueueName + ".Input", true, false, false);
        _channel.QueueDeclare(_configuration.QueueName + ".Output", true, false, false);
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (_, ea) =>
        {
            if (!_callbackMapper.TryRemove(ea.BasicProperties.CorrelationId, out TaskCompletionSource<RpcDto>? tcs))
                return;
            if (tcs == null)
                return;
            var body = ea.Body.ToArray();
            var response = new RpcDto(body);
            tcs.TrySetResult(response);
        };

        _channel.BasicConsume(
            consumer: consumer,
            queue: _configuration.QueueName + ".Output",
            autoAck: true);
    }

    public void Stop()
    {
        _channel.Close();
        _connection.Close();
    }
}