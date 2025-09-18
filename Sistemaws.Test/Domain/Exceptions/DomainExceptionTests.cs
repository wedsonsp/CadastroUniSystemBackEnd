using Sistemaws.Domain.Exceptions;

namespace Sistemaws.Test.Domain.Exceptions;

public class DomainExceptionTests
{
    [Fact]
    public void DomainException_WithEmptyErrors_ShouldInitializeCorrectly()
    {
        // Arrange
        var errors = new Dictionary<string, string>();

        // Act
        var domainException = new DomainException(errors);

        // Assert
        Assert.NotNull(domainException.Errors);
        Assert.Empty(domainException.Errors);
    }

    [Fact]
    public void DomainException_WithSingleError_ShouldInitializeCorrectly()
    {
        // Arrange
        var errors = new Dictionary<string, string> { { "Field", "Error message" } };

        // Act
        var domainException = new DomainException(errors);

        // Assert
        Assert.NotNull(domainException.Errors);
        Assert.Single(domainException.Errors);
        Assert.True(domainException.Errors.ContainsKey("Field"));
        Assert.Equal("Error message", domainException.Errors["Field"]);
    }

    [Fact]
    public void DomainException_WithMultipleErrors_ShouldInitializeCorrectly()
    {
        // Arrange
        var errors = new Dictionary<string, string> 
        { 
            { "Field1", "Error 1" }, 
            { "Field2", "Error 2" }, 
            { "Field3", "Error 3" } 
        };

        // Act
        var domainException = new DomainException(errors);

        // Assert
        Assert.NotNull(domainException.Errors);
        Assert.Equal(3, domainException.Errors.Count);
        Assert.True(domainException.Errors.ContainsKey("Field1"));
        Assert.True(domainException.Errors.ContainsKey("Field2"));
        Assert.True(domainException.Errors.ContainsKey("Field3"));
        Assert.Equal("Error 1", domainException.Errors["Field1"]);
        Assert.Equal("Error 2", domainException.Errors["Field2"]);
        Assert.Equal("Error 3", domainException.Errors["Field3"]);
    }

    [Fact]
    public void DomainException_ShouldInheritFromException()
    {
        // Arrange
        var errors = new Dictionary<string, string> { { "Field", "Error message" } };

        // Act
        var domainException = new DomainException(errors);

        // Assert
        Assert.IsAssignableFrom<Exception>(domainException);
    }

    [Fact]
    public void DomainException_WithStringMessage_ShouldInitializeCorrectly()
    {
        // Arrange
        var message = "General error message";

        // Act
        var domainException = new DomainException(message);

        // Assert
        Assert.NotNull(domainException.Errors);
        Assert.Single(domainException.Errors);
        Assert.True(domainException.Errors.ContainsKey("General"));
        Assert.Equal(message, domainException.Errors["General"]);
    }

    [Theory]
    [InlineData("Email", "Email é obrigatório")]
    [InlineData("Password", "Senha é obrigatória")]
    [InlineData("Email", "Email deve ter um formato válido")]
    [InlineData("Password", "Senha deve ter no mínimo 6 caracteres")]
    public void DomainException_WithValidationErrors_ShouldStoreCorrectly(string field, string errorMessage)
    {
        // Arrange
        var errors = new Dictionary<string, string> { { field, errorMessage } };

        // Act
        var domainException = new DomainException(errors);

        // Assert
        Assert.True(domainException.Errors.ContainsKey(field));
        Assert.Equal(errorMessage, domainException.Errors[field]);
    }
}
