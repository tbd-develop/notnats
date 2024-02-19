namespace parsers.tests;

public class when_command_is_in_uppercase
{
    [Fact]
    public void command_is_as_received()
    {
        // Arrange
        var parser = new CommandParser();
        
        // Act
        var result = parser.Parse("COMMAND arg1 arg2");
        
        // Assert
        Assert.Equal("COMMAND", result.Value);
    }
}