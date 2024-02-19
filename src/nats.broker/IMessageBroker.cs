namespace nats.broker;

public interface IMessageBroker
{
    void Subscribe(string subject, NatsClient client);
    ValueTask PublishAsync(string subject, string message, CancellationToken cancellationToken);
}