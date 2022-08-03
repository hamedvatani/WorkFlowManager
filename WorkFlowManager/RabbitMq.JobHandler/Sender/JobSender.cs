using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace RabbitMq.JobHandler.Sender;

public class JobSender : IJobSender
{
    private readonly JobHandlerConfiguration _configuration;
    private ConnectionFactory _factory = null!;
    private IConnection _connection = null!;
    private IModel _channel = null!;

    public JobSender(JobHandlerConfiguration configuration)
    {
        this._configuration = configuration;
        CreateRabbitMqConnection();
    }

    public JobSender(Action<JobHandlerConfiguration> configBuilder)
    {
        _configuration = new JobHandlerConfiguration();
        configBuilder(_configuration);
        CreateRabbitMqConnection();
    }

    private void CreateRabbitMqConnection()
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
    }

    public Task SendJobAsync<T>(T job)
    {
        return Task.Run(() =>
        {
            var str = JsonConvert.SerializeObject(job);
            var bytes = Encoding.UTF8.GetBytes(str);
            var props = _channel.CreateBasicProperties();
            props.Persistent = _configuration.Durable;
            _channel.BasicPublish("", _configuration.GroupName, props, bytes);
        });
    }
}