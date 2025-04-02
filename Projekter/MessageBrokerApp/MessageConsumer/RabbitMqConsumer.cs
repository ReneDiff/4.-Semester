using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using MessageShared;
using Microsoft.Extensions.Logging; // Tilføj logging
using System; // Tilføj for IDisposable

namespace MessageConsumer;

// Lytter på RabbitMQ og håndterer beskeder via MessageHandler og DB.

public class RabbitMqConsumer
{
    private readonly IModel _channel;
    private readonly IConnection _connection; // Gem forbindelsen for at kunne dispose
    private readonly IMessageHandler _handler;
    private readonly IDatabaseService _database;
    private readonly ILogger<RabbitMqConsumer> _logger; // Tilføj logger
    private const string QueueName = "Message-Queue";
    private string? _consumerTag; // Til at stoppe consumeren

    public RabbitMqConsumer(IMessageHandler handler, IDatabaseService database, ILogger<RabbitMqConsumer> logger)
    {
        _handler = handler;
        _database = database;
        _logger = logger; // Gem logger

        try
        {
            // Opsæt forbindelse (overvej at flytte hostname til konfiguration senere)
            var factory = new ConnectionFactory() { HostName = "localhost" }; // Hardkodet for nu
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: QueueName,
                                durable: false, // Skal ændres senere for persistens
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);
            _logger.LogInformation("RabbitMQ connection and channel established. Queue '{QueueName}' declared.", QueueName);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Failed to connect or declare queue '{QueueName}'. Consumer cannot start.", QueueName);
            // Kast undtagelsen videre, så Host'en ved, at opstart fejlede
            throw;
        }
    }

    public void StartConsuming() // Omdøbt fra Start for klarhed
        {
             if (_channel == null)
             {
                  _logger.LogError("Cannot start consuming, channel is not available.");
                  return;
             }

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                Message? message = null; // Brug nullable reference type

                try
                {
                    message = JsonSerializer.Deserialize<Message>(body);

                    if (message == null)
                    {
                        _logger.LogWarning("Failed to parse message. Discarding. Body: {BodyString}", Encoding.UTF8.GetString(body));
                        // Overvej at sende til DLQ her i stedet for bare at logge
                        _channel.BasicAck(ea.DeliveryTag, false); // Manuel Ack selvom vi kasserer
                        return;
                    }

                    _logger.LogDebug("Received message: Counter={Counter}, Timestamp={Timestamp}", message.Counter, message.Timestamp);

                    var action = _handler.HandleMessage(message); // Antager synkron for nu

                    switch (action)
                    {
                        case MessageHandlingResult.Discard:
                            _logger.LogInformation("Discarding message. Counter: {Counter}, Timestamp: {Timestamp}", message.Counter, message.Timestamp);
                            _channel.BasicAck(ea.DeliveryTag, false); // Manuel Ack
                            break;

                        case MessageHandlingResult.SaveToDatabase:
                            try
                            {
                                _database.SaveMessage(message); // Antager synkron for nu
                                _logger.LogInformation("Saved message to DB. Counter: {Counter}, Timestamp: {Timestamp}", message.Counter, message.Timestamp);
                                _channel.BasicAck(ea.DeliveryTag, false); // Manuel Ack
                            }
                            catch (Exception dbEx)
                            {
                                _logger.LogError(dbEx, "Error saving message to DB. Counter: {Counter}. Nacking message.", message.Counter);
                                // Nack uden requeue - overvej DLQ
                                _channel.BasicNack(ea.DeliveryTag, false, false);
                            }
                            break;

                        case MessageHandlingResult.RequeueWithIncrement:
                            message.Counter++;
                            _logger.LogInformation("Requeuing message with Counter={Counter}, Timestamp={Timestamp}", message.Counter, message.Timestamp);
                            var requeuedBody = JsonSerializer.SerializeToUtf8Bytes(message);

                            try
                            {
                                 // Genudgiv til køen
                                _channel.BasicPublish(exchange: "",
                                                      routingKey: QueueName,
                                                      basicProperties: null, // Overvej persistent her senere
                                                      body: requeuedBody);
                                _channel.BasicAck(ea.DeliveryTag, false); // Ack den oprindelige besked
                            }
                            catch (Exception pubEx)
                            {
                                 _logger.LogError(pubEx, "Error republishing message. Counter: {Counter}. Nacking original message.", message.Counter);
                                 // Nack den oprindelige besked uden requeue
                                 _channel.BasicNack(ea.DeliveryTag, false, false);
                            }
                            break;
                    }
                }
                catch (JsonException jsonEx)
                {
                    _logger.LogError(jsonEx, "JSON Deserialization error. Discarding message. Body: {BodyString}", Encoding.UTF8.GetString(body));
                    _channel.BasicAck(ea.DeliveryTag, false); // Ack for at fjerne fra køen
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error processing message: {Counter}. Nacking message.", message?.Counter ?? -1);
                    // Generel fejl - Nack uden requeue for at undgå loops
                     _channel.BasicNack(ea.DeliveryTag, false, false);
                }
            };

            // Start consuming med Manuel Ack
            _consumerTag = _channel.BasicConsume(queue: QueueName,
                                   autoAck: false, // VIGTIGT: Skift til manuel ack
                                   consumer: consumer);

            _logger.LogInformation("[Consumer] Listening for messages on queue '{QueueName}'...", QueueName);
        }

         public void StopConsuming()
        {
            if (_consumerTag != null && _channel != null && _channel.IsOpen)
            {
                _logger.LogInformation("Stopping consumer with tag: {ConsumerTag}", _consumerTag);
                _channel.BasicCancel(_consumerTag);
            }
        }

        public void Dispose()
        {
            _logger.LogInformation("Disposing RabbitMqConsumer...");
            try
            {
                // Stop consuming før kanalen lukkes
                StopConsuming();
                _channel?.Close();
                _channel?.Dispose();
            }
            catch (Exception ex)
            {
                 _logger.LogError(ex, "Error during channel close/dispose.");
            }

             try
             {
                _connection?.Close();
                _connection?.Dispose();
             }
             catch(Exception ex)
             {
                _logger.LogError(ex, "Error during connection close/dispose.");
             }
             _logger.LogInformation("RabbitMqConsumer disposed.");
        }
}