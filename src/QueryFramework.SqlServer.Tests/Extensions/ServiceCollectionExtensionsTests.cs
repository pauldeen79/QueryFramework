namespace QueryFramework.SqlServer.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void All_Dependencies_Can_Be_Resolved()
    {
        // Act
        var action = new Action(() => _ = new ServiceCollection()
            .AddQueryFrameworkSqlServer()
            .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true }));

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void QueryPagedDatabaseCommandProvider_Is_Returned_When_No_Custom_PagedDatabaseCommandProviderProvider_Is_Registered()
    {
        // Arrange
        using var provider = new ServiceCollection()
            .AddQueryFrameworkSqlServer()
            .BuildServiceProvider();
        var sut = provider.GetRequiredService<IPagedDatabaseCommandProviderFactory>();
        var query = new Mock<ISingleEntityQuery>().Object;

        // Act
        var actual = sut.Create(query);

        // Assert
        actual.Should().BeOfType<QueryPagedDatabaseCommandProvider>();
    }

    [Fact]
    public void Custom_PagedDatabaseCommandProvider_Is_Returned_When_Custom_PagedDatabaseCommandProviderProvider_Is_Registered()
    {
        // Arrange
        var customPagedDatabaseCommandProviderProviderMock = new Mock<IPagedDatabaseCommandProviderProvider>();
        var customCommandProvider = new Mock<IPagedDatabaseCommandProvider<ISingleEntityQuery>>().Object;
        customPagedDatabaseCommandProviderProviderMock.Setup(x => x.TryCreate(It.IsAny<ISingleEntityQuery>(), out customCommandProvider))
                                                      .Returns(true);
        using var provider = new ServiceCollection()
            .AddQueryFrameworkSqlServer(x => x.AddSingleton(customPagedDatabaseCommandProviderProviderMock.Object))
            .BuildServiceProvider();
        var sut = provider.GetRequiredService<IPagedDatabaseCommandProviderFactory>();
        var query = new Mock<ISingleEntityQuery>().Object;

        // Act
        var actual = sut.Create(query);

        // Assert
        actual.Should().BeSameAs(customCommandProvider);
    }
}
