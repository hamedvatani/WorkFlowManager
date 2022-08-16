using System.Xml;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using WorkFlowManager.Client;

namespace WorkFlowManager.Core.Rpc;

public class RpcServer
{
    private readonly ManagerConfiguration _configuration;
    private IConnection _connection = null!;
    private IModel _channel = null!;
    private EventingBasicConsumer _consumer = null!;

    public RpcServer(ManagerConfiguration configuration)
    {
        _configuration = configuration;
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
        _consumer = new EventingBasicConsumer(_channel);

        RpcDto response = new RpcDto("");

        _consumer.Received += (_, ea) =>
        {
            var body = ea.Body.ToArray();
            var props = ea.BasicProperties;
            var replyProps = _channel.CreateBasicProperties();
            replyProps.CorrelationId = props.CorrelationId;

            try
            {
                var message = new RpcDto(body);
                response = DoFunction(message);
            }
            catch (Exception)
            {
                response = new RpcDto("");
            }
            finally
            {
                _channel.BasicPublish(exchange: "", routingKey: _configuration.QueueName + ".Output",
                    basicProperties: replyProps, body: response.Serialize());
                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            }
        };

        _channel.BasicConsume(
            consumer: _consumer,
            queue: _configuration.QueueName + ".Input",
            autoAck: false);
    }

    public void Stop()
    {
        _channel.Close();
        _connection.Close();
    }

    public RpcDto DoFunction(RpcDto dto)
    {
        dto.Parameters.Add("K4","V4");
        return dto;
    }
}