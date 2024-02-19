namespace parsers.tests;

public class when_command_has_no_arguments_and_carriage_return
{
    [Fact]
    public void command_is_as_received()
    {
        // Arrange
        var parser = new CommandParser();

        // Act
        var result = parser.Parse("command\r\n");

        // Assert
        Assert.Equal("command", result.Value);
    }

    [Fact]
    public void arguments_is_empty()
    {
        // Arrange
        var parser = new CommandParser();
        
        // Act
        var result = parser.Parse("command\r\n");
        
        // Assert
        
        Assert.Empty(result.Arguments);
    }
}