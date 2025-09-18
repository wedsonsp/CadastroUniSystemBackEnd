using Sistemaws.Application.Queries;

namespace Sistemaws.Test.Application.Queries;

public class GetUserByIdQueryTests
{
    [Fact]
    public void GetUserByIdQuery_DefaultValue_ShouldBeZero()
    {
        // Arrange & Act
        var query = new GetUserByIdQuery();
        
        // Assert
        Assert.Equal(0, query.Id);
    }

    [Fact]
    public void GetUserByIdQuery_WithValidId_ShouldSetProperty()
    {
        // Arrange
        var id = 1;

        // Act
        var query = new GetUserByIdQuery { Id = id };
        
        // Assert
        Assert.Equal(id, query.Id);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(999)]
    [InlineData(int.MaxValue)]
    public void GetUserByIdQuery_WithDifferentIds_ShouldSetCorrectly(int id)
    {
        // Arrange & Act
        var query = new GetUserByIdQuery { Id = id };
        
        // Assert
        Assert.Equal(id, query.Id);
    }
}
