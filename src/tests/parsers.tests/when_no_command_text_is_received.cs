namespace parsers.tests;

public class when_no_command_text_is_received
{
    [Fact]
    public void command_is_invalid()
    {
        // Arrange
        var parser = new CommandParser();

        // Act
        var result = parser.Parse(string.Empty);

        // Assert
        Assert.True(result.IsInvalid);
    }
}