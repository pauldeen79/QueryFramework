namespace QueryFramework.SqlServer.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void All_Dependencies_Can_Be_Resolved()
    {
        // Act
        var action = new Action(() => _ = new ServiceCollection()
            .AddExpressionFramework()
            .AddQueryFrameworkSqlServer()
            .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true }));

        // Assert
        action.Should().NotThrow();
    }
}
