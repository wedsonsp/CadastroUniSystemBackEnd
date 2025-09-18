using Sistemaws.Application.Queries;

namespace Sistemaws.Test.Application.Queries;

public class GetAllUsersQueryTests
{
    [Fact]
    public void GetAllUsersQuery_ShouldBeCreated()
    {
        // Act
        var query = new GetAllUsersQuery();

        // Assert
        Assert.NotNull(query);
    }

    [Fact]
    public void GetAllUsersQuery_ShouldBeInstantiable()
    {
        // Act
        var query = new GetAllUsersQuery();

        // Assert
        Assert.IsType<GetAllUsersQuery>(query);
    }
}
