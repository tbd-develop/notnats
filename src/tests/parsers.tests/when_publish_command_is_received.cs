namespace parsers.tests;

public class when_publish_command_is_received
{
    [Fact]
    public void arguments_contains_message_content()
    {
        // Arrange
        var parser = new CommandParser();
        
        // Act
        var command = parser.Parse("PUB subject 5\r\nhello\r\n");
        
        // Assert
        
        Assert.Equal("PUB", command.Value);
        Assert.Equal("subject", command.Arguments[0]);
        Assert.Equal("hello", command.Arguments[2]);
    }
}