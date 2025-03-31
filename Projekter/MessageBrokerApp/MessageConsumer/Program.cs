using MessageConsumer;
using MessageShared;
var handler = new MessageHandler();
var db = new PostgresService("Host=localhost;Port=5432;Username=appuser;Password=secret;Database=messagesdb");

// Tving en besked med lige sekund og frisk timestamp
var testMessage = new Message
{
    Timestamp = DateTime.UtcNow.AddSeconds(-(DateTime.UtcNow.Second % 2)), // sikrer lige sekund
    Counter = 1
};

var action = handler.HandleMessage(testMessage);
Console.WriteLine($"Handler returned: {action}");

if (action == MessageHandlingResult.SaveToDatabase)
{
    db.SaveMessage(testMessage);
}

/*
var handler = new MessageHandler();
var db = new PostgresService("Host=localhost;Port=5432;Username=appuser;Password=secret;Database=messagesdb");
var consumer = new RabbitMqConsumer(handler, db);

consumer.Start();

// Hold programmet i live
Console.ReadLine();
*/