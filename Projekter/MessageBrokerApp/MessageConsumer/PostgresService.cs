using MessageShared;
using System;
using Npgsql;

namespace MessageConsumer;

// Gemmer beskeder i en PostgreSQL-database.
public class PostgresService : IDatabaseService
{
    private readonly string _connectionString;

    public PostgresService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void SaveMessage(Message message)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        conn.Open();

        using var cmd = new NpgsqlCommand("INSERT INTO messages (timestamp, counter) VALUES (@timestamp, @counter)", conn);

        cmd.Parameters.AddWithValue("timestamp", message.Timestamp);
        cmd.Parameters.AddWithValue("counter",message.Counter);

        cmd.ExecuteNonQuery();

        Console.WriteLine($"[DB] Gemte besked: Counter={message.Counter}, Timestamp={message.Timestamp:HH:mm:ss}");
    }
}