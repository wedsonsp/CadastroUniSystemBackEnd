using Sistemaws.Domain.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Sistemaws.Test.Domain.DTOs;

public class LoginRequestTests
{
    [Fact]
    public void LoginRequest_ShouldHaveRequiredEmailAttribute()
    {
        // Arrange
        var property = typeof(LoginRequest).GetProperty(nameof(LoginRequest.Email));
        
        // Act & Assert
        var requiredAttribute = property?.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() as RequiredAttribute;
        Assert.NotNull(requiredAttribute);
        Assert.Equal("Email é obrigatório", requiredAttribute!.ErrorMessage);
    }

    [Fact]
    public void LoginRequest_ShouldHaveEmailAddressAttribute()
    {
        // Arrange
        var property = typeof(LoginRequest).GetProperty(nameof(LoginRequest.Email));
        
        // Act & Assert
        var emailAttribute = property?.GetCustomAttributes(typeof(EmailAddressAttribute), false).FirstOrDefault() as EmailAddressAttribute;
        Assert.NotNull(emailAttribute);
        Assert.Equal("Email deve ter um formato válido", emailAttribute!.ErrorMessage);
    }

    [Fact]
    public void LoginRequest_ShouldHaveRequiredPasswordAttribute()
    {
        // Arrange
        var property = typeof(LoginRequest).GetProperty(nameof(LoginRequest.Password));
        
        // Act & Assert
        var requiredAttribute = property?.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() as RequiredAttribute;
        Assert.NotNull(requiredAttribute);
        Assert.Equal("Senha é obrigatória", requiredAttribute!.ErrorMessage);
    }

    [Fact]
    public void LoginRequest_DefaultValues_ShouldBeEmpty()
    {
        // Arrange & Act
        var loginRequest = new LoginRequest();
        
        // Assert
        Assert.Empty(loginRequest.Email);
        Assert.Empty(loginRequest.Password);
    }

    [Theory]
    [InlineData("test@example.com", "password123")]
    [InlineData("user@domain.com", "mypassword")]
    [InlineData("admin@system.com", "admin123")]
    public void LoginRequest_WithValidData_ShouldSetProperties(string email, string password)
    {
        // Arrange & Act
        var loginRequest = new LoginRequest
        {
            Email = email,
            Password = password
        };
        
        // Assert
        Assert.Equal(email, loginRequest.Email);
        Assert.Equal(password, loginRequest.Password);
    }
}
