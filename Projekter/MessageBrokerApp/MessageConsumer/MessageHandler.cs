using MessageShared;

namespace MessageConsumer;

public class MessageHandler : IMessageHandler
{
    private const int MaxRetries = 5;
    public MessageHandlingResult HandleMessage(Message message)
    {
        var now = DateTime.UtcNow;
        var age = now - message.Timestamp;

        if (age > TimeSpan.FromMinutes(1))
        {
            return MessageHandlingResult.Discard;
        }

        if (message.Timestamp.Second % 2 == 0)
        {
            return MessageHandlingResult.SaveToDatabase;
        }

        if (message.Counter >= MaxRetries)
        {
            Console.WriteLine($"[Handler] Besked opgivet efter {message.Counter} forsøg.");
            return MessageHandlingResult.Discard;
        }

        return MessageHandlingResult.RequeueWithIncrement;
    }
}