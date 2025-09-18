using Sistemaws.Domain.DTOs;

namespace Sistemaws.Test.Domain.DTOs;

public class LoginResponseTests
{
    [Fact]
    public void LoginResponse_DefaultValues_ShouldBeCorrect()
    {
        // Arrange & Act
        var loginResponse = new LoginResponse();
        
        // Assert
        Assert.Empty(loginResponse.Token);
        Assert.NotNull(loginResponse.User);
        Assert.IsType<UserResponse>(loginResponse.User);
    }

    [Fact]
    public void LoginResponse_WithValidData_ShouldSetProperties()
    {
        // Arrange
        var token = "jwt-token-here";
        var user = new UserResponse
        {
            Id = 1,
            Name = "João Silva",
            Email = "joao.silva@example.com",
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        // Act
        var loginResponse = new LoginResponse
        {
            Token = token,
            User = user
        };
        
        // Assert
        Assert.Equal(token, loginResponse.Token);
        Assert.NotNull(loginResponse.User);
        Assert.Equal(user.Id, loginResponse.User.Id);
        Assert.Equal(user.Name, loginResponse.User.Name);
        Assert.Equal(user.Email, loginResponse.User.Email);
        Assert.Equal(user.IsActive, loginResponse.User.IsActive);
    }

    [Theory]
    [InlineData("token1", "User 1", "user1@example.com")]
    [InlineData("token2", "User 2", "user2@example.com")]
    [InlineData("admin-token", "Admin User", "admin@system.com")]
    public void LoginResponse_WithDifferentData_ShouldSetCorrectly(string token, string userName, string userEmail)
    {
        // Arrange
        var user = new UserResponse
        {
            Id = 1,
            Name = userName,
            Email = userEmail,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        // Act
        var loginResponse = new LoginResponse
        {
            Token = token,
            User = user
        };
        
        // Assert
        Assert.Equal(token, loginResponse.Token);
        Assert.Equal(userName, loginResponse.User.Name);
        Assert.Equal(userEmail, loginResponse.User.Email);
    }
}
