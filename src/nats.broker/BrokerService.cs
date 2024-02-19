using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Hosting;
using nats.parser;

namespace nats.broker;

public class BrokerService(
    IMessageBroker broker,
    ICommandParser parser) : BackgroundService
{
    private List<NatsClient> _clients = new();
    private readonly List<Task> _tasks = new();

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var listener = new TcpListener(IPAddress.Loopback, 4222);

        listener.Start();

        while (!stoppingToken.IsCancellationRequested)
        {
            var client =
                new NatsClient(broker, parser, await listener.AcceptTcpClientAsync(stoppingToken));

            await client.Initialize(IPAddress.Loopback, 4222);

            _clients.Add(client);

            _tasks.Add(client.Listen(stoppingToken));
        }

        await Task.WhenAll(_tasks);

        listener.Stop();
    }
}