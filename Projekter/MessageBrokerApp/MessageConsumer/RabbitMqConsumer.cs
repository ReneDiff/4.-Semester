using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using MessageShared;
using Microsoft.Extensions.Logging; // Tilføj logging
using System; // Tilføj for IDisposable
using Microsoft.Extensions.Configuration; // Tilføjet for konfiguration


namespace MessageConsumer;

// Lytter på RabbitMQ og håndterer beskeder via MessageHandler og DB.

public class RabbitMqConsumer :IDisposable
{
    private readonly IModel _channel;
    private readonly IConnection _connection; // Gem forbindelsen for at kunne dispose
    private readonly IMessageHandler _handler;
    private readonly IDatabaseService _database;
    private readonly ILogger<RabbitMqConsumer> _logger; // Tilføj logger
    private const string QueueName = "Message-Queue";
    private string? _consumerTag; // Til at stoppe consumeren

    public RabbitMqConsumer(IMessageHandler handler, IDatabaseService database, ILogger<RabbitMqConsumer> logger, IConfiguration configuration)
    {
        _handler = handler;
        _database = database;
        _logger = logger; // Gem logger
        // _configuration = configuration; // Gem evt. configuration hvis den skal bruges andre steder

        // Læs hostname fra konfiguration
        string hostname = configuration["RabbitMq:HostName"] ?? "localhost"; // Brug fallback
        _logger.LogInformation("Consumer connecting to RabbitMQ Host: {HostName}", hostname);

        try
        {
            // Brug hostname fra konfigurationen her
            var factory = new ConnectionFactory() { HostName = hostname };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // QueueDeclare uændret, men overvej durable: true senere
            _channel.QueueDeclare(queue: QueueName,
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);
            _logger.LogInformation("RabbitMQ connection and channel established. Queue '{QueueName}' declared.", QueueName);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Failed to connect Consumer to RabbitMQ host {HostName}", hostname);
            throw;
        }
    }

    public void StartConsuming() // Omdøbt fra Start for klarhed
    {
        if (_channel == null || !_channel.IsOpen) // Tilføjet tjek for IsOpen
            {
                _logger.LogError("Cannot start consuming, channel is not available or closed.");
                return;
            }

        var consumer = new EventingBasicConsumer(_channel);

        // ----> START ANBEFALING: GØR LAMBDA ASYNKRON <----
        // Ved at tilføje 'async' her, kan du bruge 'await' inde i handleren, f.eks. til Task.Delay
        consumer.Received += async (model, ea) =>
        // ----> SLUT ANBEFALING <----
        {
            var body = ea.Body.ToArray();
            Message? message = null;

            try
            {
                message = JsonSerializer.Deserialize<Message>(body);
                // ... (Resten af din try-blok med null-check, switch-case etc. er uændret) ...

                // Inde i: case MessageHandlingResult.RequeueWithIncrement:
                // ... (inkrementer counter, log) ...
                try
                {
                    // Brug await hvis lambda'en er async, ellers .Wait()
                    await Task.Delay(1000); // Vent 1 sekund
                        _logger.LogDebug("Delay before requeue finished for message Counter={Counter}", message.Counter);
                }
                catch (Exception delayEx)
                {
                    _logger.LogError(delayEx, "Error during delay before requeue.");
                    _channel.BasicNack(ea.DeliveryTag, false, false); // Nack uden requeue ved fejl i delay
                    return; // Afslut behandling for denne besked
                }

                var requeuedBody = JsonSerializer.SerializeToUtf8Bytes(message);
                // ... (Resten af RequeueWithIncrement-casen er uændret) ...

            }
            catch (JsonException jsonEx)
            {
                    _logger.LogError(jsonEx, "JSON Deserialization error. Discarding message. Body: {BodyString}", Encoding.UTF8.GetString(body));
                    // Sikkerhed: Ack selv ved fejl for at undgå at beskeden sidder fast hvis den *altid* fejler deserialisering
                    _channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error processing message: {Counter}. Nacking message.", message?.Counter ?? -1);
                    // Nack uden requeue for at undgå potentielle loops ved uventede fejl
                    _channel.BasicNack(ea.DeliveryTag, false, false);
            }
        };

        // BasicConsume uændret
            _consumerTag = _channel.BasicConsume(queue: QueueName,
                                autoAck: false,
                                consumer: consumer);

        _logger.LogInformation("[Consumer] Listening for messages on queue '{QueueName}'...", QueueName);   
    }

    public void StopConsuming()
    {
        if (_consumerTag != null && _channel != null && _channel.IsOpen)
        {
            _logger.LogInformation("Stopping consumer with tag: {ConsumerTag}", _consumerTag);
            try
            {
                _channel.BasicCancel(_consumerTag);
            } catch (Exception ex) {
                _logger.LogWarning(ex, "Exception during BasicCancel for consumer tag {ConsumerTag}", _consumerTag);
            }
        }
    }

    public void Dispose()
    {
        _logger.LogInformation("Disposing RabbitMqConsumer...");
        try
        {
            StopConsuming(); // Sørg for at stoppe consumer før lukning
            _channel?.Close();
            _channel?.Dispose();
        }
        catch (Exception ex) { _logger.LogWarning(ex, "Error disposing RabbitMQ channel."); }
        try
        {
            _connection?.Close();
            _connection?.Dispose();
        }
        catch (Exception ex) { _logger.LogWarning(ex, "Error disposing RabbitMQ connection."); }
        _logger.LogInformation("RabbitMqConsumer disposed.");
    }
}