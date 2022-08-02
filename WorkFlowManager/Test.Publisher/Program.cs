using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory
{
    HostName = "localhost"
};
var connection = factory.CreateConnection();
var channel = connection.CreateModel();

// channel.QueueDeclare(queue: "my-queue", exclusive: false);
// channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
// Console.WriteLine("Press enter to send");
// Console.ReadLine();
// for (int i = 0; i < 100; i++)
// {
//      var str = $"Message {i}";
//      var body=Encoding.UTF8.GetBytes(str);
//      channel.BasicPublish(exchange: "", routingKey: "my-queue", body: body);
// }

channel.ExchangeDeclare("my-exchange", "x-delayed-message", true, false,
     new Dictionary<string, object> {{"x-delayed-type", "direct"}});
channel.QueueDeclare("my-queue", exclusive: false, autoDelete: false);
channel.QueueBind(queue: "my-queue", exchange: "my-exchange", routingKey: "");
Console.WriteLine("Press enter to send");
Console.ReadLine();
var str = $"Message Send Time : {DateTime.Now}";
var body = Encoding.UTF8.GetBytes(str);
var props = channel.CreateBasicProperties();
props.Headers = new Dictionary<string, object>();
props.Headers.Add("x-delay", 5000);
channel.BasicPublish("my-exchange", "", body: body, basicProperties: props);

Console.WriteLine("Press enter to exit");
Console.ReadLine();
