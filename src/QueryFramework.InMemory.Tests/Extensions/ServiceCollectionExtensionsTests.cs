namespace QueryFramework.InMemory.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void All_Dependencies_Can_Be_Resolved()
    {
        // Arrange
        var collection = new ServiceCollection().AddExpressionFramework();
        // Act
        var action = new Action(() => _ = collection.AddQueryFrameworkInMemory()
            .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true }));

        // Assert
        action.Should().NotThrow();
    }
}
