namespace parsers.tests;

public class when_command_is_received_with_no_arguments
{
    [Fact]
    public void command_is_valid()
    {
        // Arrange
        var parser = new CommandParser();

        // Act
        var result = parser.Parse("command");

        // Assert
        Assert.False(result.IsInvalid);
    }

    [Fact]
    public void command_value_is_as_received()
    {
        // Arrange
        var parser = new CommandParser();

        // Act
        var result = parser.Parse("command");

        // Assert
        Assert.Equal("command", result.Value);
    }

    [Fact]
    public void arguments_is_empty()
    {
        // Arrange
        var parser = new CommandParser();

        // Act
        var result = parser.Parse("command");

        // Assert
        Assert.Empty(result.Arguments);
    }
}