using Sistemaws.Domain.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Sistemaws.Test.Domain.DTOs;

public class CreateUserRequestTests
{
    [Fact]
    public void CreateUserRequest_ShouldHaveRequiredNameAttribute()
    {
        // Arrange
        var property = typeof(CreateUserRequest).GetProperty(nameof(CreateUserRequest.Name));
        
        // Act & Assert
        var requiredAttribute = property?.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() as RequiredAttribute;
        Assert.NotNull(requiredAttribute);
        Assert.Equal("Nome é obrigatório", requiredAttribute!.ErrorMessage);
    }

    [Fact]
    public void CreateUserRequest_ShouldHaveMaxLengthNameAttribute()
    {
        // Arrange
        var property = typeof(CreateUserRequest).GetProperty(nameof(CreateUserRequest.Name));
        
        // Act & Assert
        var maxLengthAttribute = property?.GetCustomAttributes(typeof(MaxLengthAttribute), false).FirstOrDefault() as MaxLengthAttribute;
        Assert.NotNull(maxLengthAttribute);
        Assert.Equal(100, maxLengthAttribute!.Length);
        Assert.Equal("Nome deve ter no máximo 100 caracteres", maxLengthAttribute.ErrorMessage);
    }

    [Fact]
    public void CreateUserRequest_ShouldHaveRequiredEmailAttribute()
    {
        // Arrange
        var property = typeof(CreateUserRequest).GetProperty(nameof(CreateUserRequest.Email));
        
        // Act & Assert
        var requiredAttribute = property?.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() as RequiredAttribute;
        Assert.NotNull(requiredAttribute);
        Assert.Equal("Email é obrigatório", requiredAttribute!.ErrorMessage);
    }

    [Fact]
    public void CreateUserRequest_ShouldHaveEmailAddressAttribute()
    {
        // Arrange
        var property = typeof(CreateUserRequest).GetProperty(nameof(CreateUserRequest.Email));
        
        // Act & Assert
        var emailAttribute = property?.GetCustomAttributes(typeof(EmailAddressAttribute), false).FirstOrDefault() as EmailAddressAttribute;
        Assert.NotNull(emailAttribute);
        Assert.Equal("Email deve ter um formato válido", emailAttribute!.ErrorMessage);
    }

    [Fact]
    public void CreateUserRequest_ShouldHaveMaxLengthEmailAttribute()
    {
        // Arrange
        var property = typeof(CreateUserRequest).GetProperty(nameof(CreateUserRequest.Email));
        
        // Act & Assert
        var maxLengthAttribute = property?.GetCustomAttributes(typeof(MaxLengthAttribute), false).FirstOrDefault() as MaxLengthAttribute;
        Assert.NotNull(maxLengthAttribute);
        Assert.Equal(255, maxLengthAttribute!.Length);
        Assert.Equal("Email deve ter no máximo 255 caracteres", maxLengthAttribute.ErrorMessage);
    }

    [Fact]
    public void CreateUserRequest_ShouldHaveRequiredPasswordAttribute()
    {
        // Arrange
        var property = typeof(CreateUserRequest).GetProperty(nameof(CreateUserRequest.Password));
        
        // Act & Assert
        var requiredAttribute = property?.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() as RequiredAttribute;
        Assert.NotNull(requiredAttribute);
        Assert.Equal("Senha é obrigatória", requiredAttribute!.ErrorMessage);
    }

    [Fact]
    public void CreateUserRequest_ShouldHaveMinLengthPasswordAttribute()
    {
        // Arrange
        var property = typeof(CreateUserRequest).GetProperty(nameof(CreateUserRequest.Password));
        
        // Act & Assert
        var minLengthAttribute = property?.GetCustomAttributes(typeof(MinLengthAttribute), false).FirstOrDefault() as MinLengthAttribute;
        Assert.NotNull(minLengthAttribute);
        Assert.Equal(6, minLengthAttribute!.Length);
        Assert.Equal("Senha deve ter no mínimo 6 caracteres", minLengthAttribute.ErrorMessage);
    }

    [Fact]
    public void CreateUserRequest_DefaultValues_ShouldBeEmpty()
    {
        // Arrange & Act
        var createUserRequest = new CreateUserRequest();
        
        // Assert
        Assert.Empty(createUserRequest.Name);
        Assert.Empty(createUserRequest.Email);
        Assert.Empty(createUserRequest.Password);
    }

    [Theory]
    [InlineData("João Silva", "joao@example.com", "password123")]
    [InlineData("Maria Santos", "maria@domain.com", "mypassword")]
    [InlineData("Admin User", "admin@system.com", "admin123")]
    public void CreateUserRequest_WithValidData_ShouldSetProperties(string name, string email, string password)
    {
        // Arrange & Act
        var createUserRequest = new CreateUserRequest
        {
            Name = name,
            Email = email,
            Password = password
        };
        
        // Assert
        Assert.Equal(name, createUserRequest.Name);
        Assert.Equal(email, createUserRequest.Email);
        Assert.Equal(password, createUserRequest.Password);
    }
}
