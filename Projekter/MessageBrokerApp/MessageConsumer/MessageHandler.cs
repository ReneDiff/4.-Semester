using MessageShared;

namespace MessageConsumer;

public class MessageHandler : IMessageHandler
{
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

        return MessageHandlingResult.RequeueWithIncrement;
    }
}