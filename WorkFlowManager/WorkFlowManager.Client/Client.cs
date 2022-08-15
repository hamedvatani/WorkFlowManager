using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using WorkFlowManager.Client.Models;

namespace WorkFlowManager.Client;

public class Client
{
    private ClientConfiguration _configuration;
    private ConnectionFactory _factory = null!;
    private IConnection _connection = null!;
    private IModel _channel = null!;
    private EventingBasicConsumer _consumer = null!;

    public Client(ClientConfiguration configuration)
    {
        this._configuration = configuration;

        _factory = new ConnectionFactory
        {
            HostName = _configuration.RabbitMqHostName,
            Port = _configuration.RabbitMqPort,
            UserName = _configuration.RabbitMqUserName,
            Password = _configuration.RabbitMqPassword
        };
        _connection = _factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(_configuration.QueueName + ".Output", true, false, false);
        _channel.BasicQos(0, 1, false);
        _consumer = new EventingBasicConsumer(_channel);
        _consumer.Received += onReceiveMessage;
        _channel.BasicConsume(_configuration.QueueName + ".Output", false, _consumer);
    }

    private void onReceiveMessage(object? sender, BasicDeliverEventArgs e)
    {
    }

    public WorkFlow? GetWorkFlow(string name)
    {
    }
}