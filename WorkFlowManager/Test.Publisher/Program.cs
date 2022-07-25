using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory
{
    HostName = "localhost"
};
var connection = factory.CreateConnection();
var channel = connection.CreateModel();

channel.QueueDeclare(queue: "my-queue", exclusive: false);

Console.WriteLine("Press enter to send");
Console.ReadLine();

for (int i = 0; i < 100; i++)
{
    var str = $"Message {i}";
    var body=Encoding.UTF8.GetBytes(str);
    channel.BasicPublish(exchange: "", routingKey: "my-queue", body: body);
}

Console.WriteLine("Press enter to exit");
Console.ReadLine();
