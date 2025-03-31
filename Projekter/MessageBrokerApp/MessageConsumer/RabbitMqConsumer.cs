using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using MessageShared;

namespace MessageConsumer;

// Lytter på RabbitMQ og håndterer beskeder via MessageHandler og DB.

public class RabbitMqConsumer
{
    private readonly IModel _channel;
    private readonly IMessageHandler _handler;
    private readonly IDatabaseService _database;
    private const string QueueName = "Message-Queue";

    public RabbitMqConsumer(IMessageHandler handler, IDatabaseService database)
    {
        _handler = handler;
        _database = database;

        //opsætter forbindelse til RabbitMq(lokalt)
        var factory = new ConnectionFactory() {HostName = "localhost"};
        var _conection = factory.CreateConnection();
        _channel = _conection.CreateModel();

        // Sørg for at køen findes
        _channel.QueueDeclare(queue: QueueName,
                              durable: false,
                              exclusive: false,
                              autoDelete: false,
                              arguments: null);
    }

    public void Start()
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (model, ea) => 
        {
            var body = ea.Body.ToArray();
            var message = JsonSerializer.Deserialize<Message>(body);

            if(message == null)
            {
                Console.WriteLine("[Consumer] Fejl: Kunne ikke parse besked.");
                return;
            }

            var action = _handler.HandleMessage(message);

            switch(action)
            {
                case MessageHandlingResult.Discard:
                    Console.WriteLine("[Consumer] Kassér besked.");
                    break;
                
                case MessageHandlingResult.SaveToDatabase:
                    _database.SaveMessage(message);
                    break;

                case MessageHandlingResult.RequeueWithIncrement:
                    message.Counter++;
                    var requeuedBody = JsonSerializer.SerializeToUtf8Bytes(message);

                    _channel.BasicPublish(exchange: "",
                    routingKey: QueueName,
                    basicProperties: null,
                    body: requeuedBody);

                    Console.WriteLine($"[Consumer] Requeued besked med Counter={message.Counter}");
                    break;
            }
        };

        _channel.BasicConsume(queue: QueueName,
                              autoAck: true,
                              consumer: consumer);

        Console.WriteLine("[Consumer] Lytter på beskeder...");
    }
}