namespace Sistemaws.Test;

public class UnitTest1
{
    [Fact]
    public void ProjectSetup_ShouldBeValid()
    {
        // Arrange & Act
        var projectName = "Sistemaws.Test";
        
        // Assert
        Assert.NotNull(projectName);
        Assert.NotEmpty(projectName);
        Assert.Contains("Test", projectName);
    }

    [Fact]
    public void BasicMath_ShouldWork()
    {
        // Arrange
        var a = 2;
        var b = 3;
        
        // Act
        var result = a + b;
        
        // Assert
        Assert.Equal(5, result);
    }
}