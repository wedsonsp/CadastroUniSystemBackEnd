using Sistemaws.Domain.DTOs;

namespace Sistemaws.Test.Domain.DTOs;

public class UserResponseTests
{
    [Fact]
    public void UserResponse_DefaultValues_ShouldBeCorrect()
    {
        // Arrange & Act
        var userResponse = new UserResponse();
        
        // Assert
        Assert.Equal(0, userResponse.Id);
        Assert.Empty(userResponse.Name);
        Assert.Empty(userResponse.Email);
        Assert.Equal(default(DateTime), userResponse.CreatedAt);
        Assert.Null(userResponse.UpdatedAt);
        Assert.False(userResponse.IsActive);
    }

    [Fact]
    public void UserResponse_WithValidData_ShouldSetProperties()
    {
        // Arrange
        var id = 1;
        var name = "João Silva";
        var email = "joao.silva@example.com";
        var createdAt = DateTime.UtcNow;
        var updatedAt = DateTime.UtcNow.AddDays(1);
        var isActive = true;

        // Act
        var userResponse = new UserResponse
        {
            Id = id,
            Name = name,
            Email = email,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            IsActive = isActive
        };
        
        // Assert
        Assert.Equal(id, userResponse.Id);
        Assert.Equal(name, userResponse.Name);
        Assert.Equal(email, userResponse.Email);
        Assert.Equal(createdAt, userResponse.CreatedAt);
        Assert.Equal(updatedAt, userResponse.UpdatedAt);
        Assert.Equal(isActive, userResponse.IsActive);
    }

    [Theory]
    [InlineData(1, "User 1", "user1@example.com")]
    [InlineData(2, "User 2", "user2@example.com")]
    [InlineData(999, "Admin User", "admin@system.com")]
    public void UserResponse_WithDifferentIds_ShouldSetCorrectly(int id, string name, string email)
    {
        // Arrange & Act
        var userResponse = new UserResponse
        {
            Id = id,
            Name = name,
            Email = email,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };
        
        // Assert
        Assert.Equal(id, userResponse.Id);
        Assert.Equal(name, userResponse.Name);
        Assert.Equal(email, userResponse.Email);
        Assert.True(userResponse.IsActive);
    }
}
