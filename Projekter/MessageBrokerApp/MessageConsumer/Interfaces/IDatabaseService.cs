using MessageShared;

namespace MessageConsumer;

// Abstraktion for at gemme beskeder i databasen.

public interface IDatabaseService
{
    void SaveMessage(Message message);
}