namespace QueryFramework.SqlServer.Tests;

public class DefaultPagedDatabaseCommandProviderFactoryTests
{
    [Fact]
    public void Create_Throws_When_Provider_Returns_Null_Result()
    {
        // Arrange
        var providerMock = new PagedDatabaseCommandProviderProviderMock();
        providerMock.ReturnValue = true;
        providerMock.ResultDelegate = new Func<ISingleEntityQuery, IPagedDatabaseCommandProvider?>(_ => null);
        var sut = new DefaultPagedDatabaseCommandProviderFactory(new[] { providerMock });
        var query = new SingleEntityQuery();

        // Act
        sut.Invoking(x => x.Create<object>(query))
           .Should().ThrowExactly<InvalidOperationException>()
           .WithMessage("Paged database command provider of data type [System.Object] for query type [QueryFramework.Core.Queries.SingleEntityQuery] provided an empty result");
    }

    [Fact]
    public void Create_Throws_When_No_Provider_Returns_True()
    {
        // Arrange
        var providerMock = new PagedDatabaseCommandProviderProviderMock();
        providerMock.ReturnValue = false;
        providerMock.ResultDelegate = new Func<ISingleEntityQuery, IPagedDatabaseCommandProvider?>(_ => null);
        var sut = new DefaultPagedDatabaseCommandProviderFactory(new[] { providerMock });
        var query = new SingleEntityQuery();

        // Act
        sut.Invoking(x => x.Create<object>(query))
           .Should().ThrowExactly<InvalidOperationException>()
           .WithMessage("Data type [System.Object] does not have a paged database command provider for query type [QueryFramework.Core.Queries.SingleEntityQuery]");
    }

    [Fact]
    public void Create_Returns_Result_Of_Provider_When_Provider_Returns_True()
    {
        // Arrange
        var providerMock = new PagedDatabaseCommandProviderProviderMock();
        var settingsMock = new Mock<IPagedDatabaseCommandProvider>();
        var pagedDatabaseCommandProvider = settingsMock.Object;
        providerMock.ReturnValue = true;
        providerMock.ResultDelegate = new Func<ISingleEntityQuery, IPagedDatabaseCommandProvider?>(_ => pagedDatabaseCommandProvider);
        var sut = new DefaultPagedDatabaseCommandProviderFactory(new[] { providerMock });
        var query = new SingleEntityQuery();

        // Act
        var actual = sut.Create<object>(query);

        // Assert
        actual.Should().BeSameAs(pagedDatabaseCommandProvider);
    }
}
