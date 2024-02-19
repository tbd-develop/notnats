namespace nats.parser;

public interface ICommandParser
{
    Command Parse(string content);
}