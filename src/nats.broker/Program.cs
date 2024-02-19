// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using nats.broker;
using nats.parser;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        services.AddSingleton<ICommandParser, CommandParser>();
        services.AddSingleton<IMessageBroker, DefaultMessageBroker>();

        services.AddHostedService<BrokerService>();
    });

var app = builder.Build();

await app.RunAsync();