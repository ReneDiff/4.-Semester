using MessageShared;
using MessageProducer;

var producer = new RabbitMqProducer();

int counter = 0;

while (true)
{
    var message = new Message
    {
        Timestamp = DateTime.UtcNow,
        Counter = counter++
    };

    producer.Send(message);

    await Task.Delay(1000); // vent 1 sekund
}
