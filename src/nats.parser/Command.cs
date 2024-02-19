namespace nats.parser;

public class Command(string command, string[] arguments, string? content = null)
{
    public bool IsInvalid => string.IsNullOrEmpty(command);
    public string Value => command;
    public string[] Arguments => arguments;
    public string? Content { get; set; } = content;
}