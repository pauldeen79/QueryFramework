namespace QueryFramework.SqlServer.Tests;

public class DefaultPagedDatabaseCommandProviderProviderTests
{
    [Fact]
    public void TryCreate_Returns_True_For_SingleEntityQuery() => TryCreate<ISingleEntityQuery>();

    [Fact]
    public void TryCreate_Returns_True_For_FieldSelectionQuery() => TryCreate<IFieldSelectionQuery>();

    [Fact]
    public void TryCreate_Returns_True_For_GroupingQuery() => TryCreate<IGroupingQuery>();

    private void TryCreate<T>() where T : class, ISingleEntityQuery
    {
        // Arrange
        var pagedDatabaseCommandProvider = new Mock<IPagedDatabaseCommandProvider<ISingleEntityQuery>>().Object;
        var sut = new DefaultPagedDatabaseCommandProviderProvider(pagedDatabaseCommandProvider);
        var query = new Mock<T>().Object;

        // Act
        var actual = sut.TryCreate(query, out var provider);

        // Assert
        actual.Should().BeTrue();
        provider.Should().BeSameAs(pagedDatabaseCommandProvider);
    }
}
