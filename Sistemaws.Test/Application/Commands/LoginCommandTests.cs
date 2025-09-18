using Sistemaws.Application.Commands;

namespace Sistemaws.Test.Application.Commands;

public class LoginCommandTests
{
    [Fact]
    public void LoginCommand_DefaultValues_ShouldBeEmpty()
    {
        // Arrange & Act
        var command = new LoginCommand();
        
        // Assert
        Assert.Empty(command.Email);
        Assert.Empty(command.Password);
    }

    [Fact]
    public void LoginCommand_WithValidData_ShouldSetProperties()
    {
        // Arrange
        var email = "test@example.com";
        var password = "password123";

        // Act
        var command = new LoginCommand
        {
            Email = email,
            Password = password
        };
        
        // Assert
        Assert.Equal(email, command.Email);
        Assert.Equal(password, command.Password);
    }

    [Theory]
    [InlineData("user1@example.com", "password1")]
    [InlineData("user2@domain.com", "password2")]
    [InlineData("admin@system.com", "admin123")]
    public void LoginCommand_WithDifferentData_ShouldSetCorrectly(string email, string password)
    {
        // Arrange & Act
        var command = new LoginCommand
        {
            Email = email,
            Password = password
        };
        
        // Assert
        Assert.Equal(email, command.Email);
        Assert.Equal(password, command.Password);
    }
}
