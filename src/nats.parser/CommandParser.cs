using System.Text.RegularExpressions;

namespace nats.parser;

public class CommandParser : ICommandParser
{
    private const string CommandPattern = @"^(?<command>[\+\-]{0,1}[\w]*)\s(?<arguments>.*)$";

    public Command Parse(string message)
    {
        if (string.IsNullOrEmpty(message))
        {
            return new Command(string.Empty, Array.Empty<string>());
        }

        var components = message.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);

        var match = Regex.Match(components[0], CommandPattern);

        var content = components.Count() > 1 ? components[1] : null;

        if (match.Success)
        {
            var command = match.Groups["command"].Value;

            var arguments = match.Groups["arguments"]
                .Value
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            return new Command(command, arguments, content);
        }

        return new Command(content, Array.Empty<string>(), content);
    }
}