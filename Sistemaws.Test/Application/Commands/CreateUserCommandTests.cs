using Sistemaws.Application.Commands;

namespace Sistemaws.Test.Application.Commands;

public class CreateUserCommandTests
{
    [Fact]
    public void CreateUserCommand_DefaultValues_ShouldBeEmpty()
    {
        // Arrange & Act
        var command = new CreateUserCommand();
        
        // Assert
        Assert.Empty(command.Name);
        Assert.Empty(command.Email);
        Assert.Empty(command.Password);
    }

    [Fact]
    public void CreateUserCommand_WithValidData_ShouldSetProperties()
    {
        // Arrange
        var name = "João Silva";
        var email = "joao.silva@example.com";
        var password = "password123";

        // Act
        var command = new CreateUserCommand
        {
            Name = name,
            Email = email,
            Password = password
        };
        
        // Assert
        Assert.Equal(name, command.Name);
        Assert.Equal(email, command.Email);
        Assert.Equal(password, command.Password);
    }

    [Theory]
    [InlineData("User 1", "user1@example.com", "password1")]
    [InlineData("User 2", "user2@domain.com", "password2")]
    [InlineData("Admin User", "admin@system.com", "admin123")]
    public void CreateUserCommand_WithDifferentData_ShouldSetCorrectly(string name, string email, string password)
    {
        // Arrange & Act
        var command = new CreateUserCommand
        {
            Name = name,
            Email = email,
            Password = password
        };
        
        // Assert
        Assert.Equal(name, command.Name);
        Assert.Equal(email, command.Email);
        Assert.Equal(password, command.Password);
    }
}
