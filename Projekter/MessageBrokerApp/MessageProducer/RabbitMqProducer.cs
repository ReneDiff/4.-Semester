using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using MessageShared;
using RabbitMQ.Client;

namespace MessageProducer;

// Ansvarlig for at sende beskeder til RabbitMQ.

public class RabbitMqProducer
{
    private readonly IConnection _conection;
    private readonly IModel _channel;
    private const string QueueName = "Message-Queue";

    public RabbitMqProducer()
    {
        //opsætter forbindelse til RabbitMq(lokalt)
        var factory = new ConnectionFactory() {HostName = "localhost"};
        _conection = factory.CreateConnection();
        _channel = _conection.CreateModel();

        // Sørg for at køen findes
        _channel.QueueDeclare(queue: QueueName,
                              durable: false,
                              exclusive: false,
                              autoDelete: false,
                              arguments: null);
    }

    //Sender besked til køen
    public void Send(Message message)
    {
        var body = JsonSerializer.SerializeToUtf8Bytes(message);
        _channel.BasicPublish(exchange: "",
        routingKey: QueueName,
        basicProperties: null,
        body: body);

        Console.WriteLine($"[Producer] Sent: Counter={message.Counter}, Time={message.Timestamp:HH:mm:ss}");
    }

    public void Dispose()
    {
        _channel?.Close();
        _conection?.Close();
    }
}