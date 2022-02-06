namespace QueryFramework.SqlServer.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void All_Dependencies_Can_Be_Resolved()
    {
        // Arrange
        var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();

        // Act
        var action = new Action(() => _ = new ServiceCollection()
            .AddQueryFrameworkSqlServer()
            .AddSingleton(settingsMock.Object)
            .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true }));

        // Assert
        action.Should().NotThrow();
    }
}
