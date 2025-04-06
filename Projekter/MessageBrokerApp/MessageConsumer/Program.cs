// MessageConsumer/Program.cs
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using MessageConsumer; // Tilføj dit namespace


public class Program
{
    public static async Task Main(string[] args)
    {
        await CreateHostBuilder(args).Build().RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                // Registrer dine services
                services.AddSingleton<IMessageHandler, MessageHandler>();

                // Registrer PostgresService - hardkodet connection string for nu
                // Næste skridt er at hente denne fra konfiguration!
                services.AddSingleton<IDatabaseService>(sp =>
                    new PostgresService("Host=postgres;Port=5433;Username=appuser;Password=secret;Database=messagesdb"));

                // Registrer RabbitMqConsumer som Singleton
                services.AddSingleton<RabbitMqConsumer>();

                // Registrer din worker service
                services.AddHostedService<ConsumerWorker>();
            });
}