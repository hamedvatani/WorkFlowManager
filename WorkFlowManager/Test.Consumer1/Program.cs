using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory
{
    HostName = "localhost"
};
var connection = factory.CreateConnection();
var channel = connection.CreateModel();

// channel.QueueDeclare(queue: "my-queue", exclusive: false);
// channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
// var consumer = new EventingBasicConsumer(channel);
// var count = 0;
// consumer.Received += (sender, ea) =>
// {
//     Console.WriteLine(Encoding.UTF8.GetString(ea.Body.ToArray()));
//     Thread.Sleep(20);
//     count++;
//     channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
// };
// channel.BasicConsume(queue: "my-queue", autoAck: false, consumer: consumer);

// channel.ExchangeDeclare("my-exchange", "x-delayed-message", true, false,
//     new Dictionary<string, object> { { "x-delayed-type", "direct" } });
// channel.QueueDeclare("my-queue", exclusive: false, autoDelete: false);
// channel.QueueBind(queue: "my-queue", exchange: "my-exchange", routingKey: "");
// channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
//
// var consumer = new EventingBasicConsumer(channel);
// var count = 0;
// consumer.Received += (sender, ea) =>
// {
//     Console.WriteLine(Encoding.UTF8.GetString(ea.Body.ToArray()) + ", Received at : " + DateTime.Now + ", Delay : " +
//                       ea.BasicProperties.Headers["x-delay"]);
//     Thread.Sleep(10);
//     count++;
//     channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
// };
// channel.BasicConsume(queue: "my-queue", autoAck: false, consumer: consumer);

channel.QueueDeclare(queue: "my-queue", exclusive: false);
channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
var consumer = new EventingBasicConsumer(channel);
var count = 0;
consumer.Received += (sender, ea) =>
{
    Console.WriteLine(Encoding.UTF8.GetString(ea.Body.ToArray()));
    channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true);
};
channel.BasicConsume(queue: "my-queue", autoAck: false, consumer: consumer);

Console.WriteLine("Listening ... Press enter to exit");
Console.ReadLine();
Console.WriteLine($"Count : {count}");
Console.ReadLine();
