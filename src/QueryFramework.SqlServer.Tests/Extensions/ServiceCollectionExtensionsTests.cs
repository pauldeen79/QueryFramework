namespace QueryFramework.SqlServer.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void All_Dependencies_Can_Be_Resolved()
    {
        // Arrange
        var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
        var settingsProviderMock = new Mock<IPagedDatabaseEntityRetrieverSettingsProvider>();
        var settings = settingsMock.Object;
        settingsProviderMock.Setup(x => x.TryCreate(It.IsAny<ISingleEntityQuery>(), out settings))
                            .Returns(true);

        // Act
        var action = new Action(() => _ = new ServiceCollection()
            .AddQueryFrameworkSqlServer()
            .AddSingleton(settingsProviderMock.Object)
            .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true }));

        // Assert
        action.Should().NotThrow();
    }
}
