using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory
{
    HostName = "localhost"
};
var connection = factory.CreateConnection();
var channel = connection.CreateModel();

channel.QueueDeclare(queue: "my-queue", exclusive: false);

var consumer = new EventingBasicConsumer(channel);
var count = 0;
consumer.Received += (sender, ea) =>
{
    Console.WriteLine(Encoding.UTF8.GetString(ea.Body.ToArray()));
    count++;
};
channel.BasicConsume(queue: "my-queue", autoAck: true, consumer: consumer);

Console.WriteLine("Listening ... Press enter to exit");
Console.ReadLine();
Console.WriteLine($"Count : {count}");
Console.ReadLine();
