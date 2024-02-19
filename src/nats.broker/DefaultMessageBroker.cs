namespace nats.broker;

public class DefaultMessageBroker : IMessageBroker
{
    private readonly Dictionary<string, List<NatsClient>> _subscribers = new();

    public void Subscribe(string subject, NatsClient client)
    {
        if (!_subscribers.ContainsKey(subject))
        {
            _subscribers.Add(subject, new List<NatsClient>());
        }

        if (!_subscribers[subject].Contains(client))
        {
            _subscribers[subject].Add(client);
        }
    }

    public async ValueTask PublishAsync(string subject, string message, CancellationToken cancellationToken)
    {
        if (!_subscribers.ContainsKey(subject))
        {
            return;
        }

        var clients = _subscribers[subject];

        foreach (var client in clients)
        {
            await client.SendMessage(subject, message, cancellationToken);
        }
    }
}