using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using nats.parser;

namespace nats.broker;

public class NatsClient(
    IMessageBroker broker,
    ICommandParser parser,
    TcpClient client) : IDisposable
{
    private bool _isReady = false;
    private PublishMessage? _publishMessage = null;

    private readonly StreamReader _reader = new(client.GetStream(), Encoding.UTF8);
    private readonly StreamWriter _writer = new(client.GetStream(), Encoding.UTF8);

    public async Task Initialize(IPAddress hostIp, int port)
    {
        var message = new
        {
            host = hostIp.ToString(),
            port,
            client_ip = (client.Client.RemoteEndPoint as IPEndPoint)?.Address.ToString()
        };

        await _writer.WriteLineAsync("INFO " + JsonSerializer.Serialize(message));
        await _writer.FlushAsync();
    }

    public async Task SendMessage(string subject, string message, CancellationToken cancellationToken)
    {
        await _writer.WriteLineAsync($"MSG {subject} {message.Length}\r\n{message}\r\n");

        await _writer.FlushAsync(cancellationToken);
    }

    public async Task Listen(CancellationToken cancellationToken)
    {
        await foreach (string message in ReceiveContent(cancellationToken))
        {
            if (_publishMessage is not null)
            {
                _publishMessage.Content = message;

                await broker.PublishAsync(_publishMessage.Topic, _publishMessage.Content, cancellationToken);

                await SendOk(cancellationToken);

                _publishMessage = null;

                continue;
            }

            var command = parser.Parse(message);

            if (command.IsInvalid)
                continue;

            if (!_isReady)
            {
                if (command.Value.Equals("CONNECT", StringComparison.CurrentCultureIgnoreCase))
                {
                    await SendOk(cancellationToken);

                    _isReady = true;
                }

                continue;
            }

            if (command.Value.Equals("PING", StringComparison.CurrentCultureIgnoreCase))
            {
                await _writer.WriteLineAsync("PONG");
                await _writer.FlushAsync(cancellationToken);

                continue;
            }

            if (command.Value.Equals("SUB", StringComparison.CurrentCultureIgnoreCase))
            {
                broker.Subscribe(command.Arguments[0], this);

                await SendOk(cancellationToken);

                continue;
            }

            if (command.Value.Equals("PUB", StringComparison.CurrentCultureIgnoreCase))
            {
                _publishMessage = new PublishMessage() { Topic = command.Arguments[0] };
            }
        }
    }

    private async Task SendOk(CancellationToken cancellationToken)
    {
        await _writer.WriteLineAsync("+OK");
        await _writer.FlushAsync(cancellationToken);
    }

    private async IAsyncEnumerable<string> ReceiveContent(
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        while (await _reader.ReadLineAsync(cancellationToken) is { } line)
        {
            yield return line;
        }
    }

    public void Dispose()
    {
        _reader.Dispose();
        _writer.Dispose();
        client.Dispose();
    }
}

public class PublishMessage
{
    public string Topic { get; set; }
    public string ReplyTo { get; set; }
    public string Content { get; set; }
}