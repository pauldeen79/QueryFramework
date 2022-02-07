namespace QueryFramework.SqlServer.Tests;

public class DefaultPagedDatabaseCommandProviderFactoryTests
{
    [Fact]
    public void Create_Throws_When_Provider_Returns_Null_Result()
    {
        // Arrange
        var pagedDatabaseCommandProviderMock = new PagedDatabaseCommandProviderProviderMock();
        pagedDatabaseCommandProviderMock.ReturnValue = true;
        pagedDatabaseCommandProviderMock.ResultDelegate = new Func<ISingleEntityQuery, IPagedDatabaseCommandProvider<ISingleEntityQuery>?>(_ => null);
        var sut = new DefaultPagedDatabaseCommandProviderFactory(new[] { pagedDatabaseCommandProviderMock });
        var query = new SingleEntityQuery();

        // Act
        sut.Invoking(x => x.Create(query))
           .Should().ThrowExactly<InvalidOperationException>()
           .WithMessage("Paged database command provider of query type [QueryFramework.Core.Queries.SingleEntityQuery] provided an empty result");
    }

    [Fact]
    public void Create_Throws_When_No_Provider_Returns_True()
    {
        // Arrange
        var pagedDatabaseCommandProviderMock = new PagedDatabaseCommandProviderProviderMock();
        pagedDatabaseCommandProviderMock.ReturnValue = false;
        pagedDatabaseCommandProviderMock.ResultDelegate = new Func<ISingleEntityQuery, IPagedDatabaseCommandProvider<ISingleEntityQuery>?>(_ => null);
        var sut = new DefaultPagedDatabaseCommandProviderFactory(new[] { pagedDatabaseCommandProviderMock });
        var query = new SingleEntityQuery();

        // Act
        sut.Invoking(x => x.Create(query))
           .Should().ThrowExactly<InvalidOperationException>()
           .WithMessage("Query type [QueryFramework.Core.Queries.SingleEntityQuery] does not have a paged database command provider");
    }

    [Fact]
    public void Create_Returns_Result_Of_Provider_When_Provider_Returns_True()
    {
        // Arrange
        var pagedDatabaseCommandProviderMock = new PagedDatabaseCommandProviderProviderMock();
        var commandProviderMock = new Mock<IPagedDatabaseCommandProvider<ISingleEntityQuery>>();
        var pagedDatabaseCommandProvider = commandProviderMock.Object;
        pagedDatabaseCommandProviderMock.ReturnValue = true;
        pagedDatabaseCommandProviderMock.ResultDelegate = new Func<ISingleEntityQuery, IPagedDatabaseCommandProvider<ISingleEntityQuery>?>(_ => pagedDatabaseCommandProvider);
        var sut = new DefaultPagedDatabaseCommandProviderFactory(new[] { pagedDatabaseCommandProviderMock });
        var query = new SingleEntityQuery();

        // Act
        var actual = sut.Create(query);

        // Assert
        actual.Should().BeSameAs(pagedDatabaseCommandProvider);
    }
}
