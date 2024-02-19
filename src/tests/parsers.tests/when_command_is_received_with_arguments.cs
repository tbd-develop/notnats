namespace parsers.tests;

public class when_command_is_received_with_arguments
{
    [Fact]
    public void command_is_valid()
    {
        // Arrange
        var parser = new CommandParser();

        // Act
        var result = parser.Parse("command arg1 arg2");

        // Assert
        Assert.False(result.IsInvalid);
    }

    [Fact]
    public void command_value_is_as_received()
    {
        // Arrange
        var parser = new CommandParser();

        // Act
        var result = parser.Parse("command arg1 arg2");

        // Assert
        Assert.Equal("command", result.Value);
    }

    [Fact]
    public void arguments_is_not_empty()
    {
        // Arrange
        var parser = new CommandParser();

        // Act
        var result = parser.Parse("command arg1 arg2");

        // Assert
        Assert.NotEmpty(result.Arguments);
    }

    [Fact]
    public void arguments_are_mapped()
    {
        // Arrange
        var parser = new CommandParser();

        // Act
        var result = parser.Parse("command arg1 arg2");

        // Assert
        Assert.Equal("arg1", result.Arguments[0]);
        Assert.Equal("arg2", result.Arguments[1]);
    }
}