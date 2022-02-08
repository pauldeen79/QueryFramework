namespace QueryFramework.SqlServer.Tests;

public class DefaultPagedDatabaseCommandProviderProviderTests
{
    [Fact]
    public void TryCreate_Returns_True_When_Query_Is_Of_Correct_Type()
    {
        // Arrange
        var pagedDatabaseCommandProvider = new Mock<IPagedDatabaseCommandProvider<ISingleEntityQuery>>().Object;
        var sut = new DefaultPagedDatabaseCommandProviderProvider<IFieldSelectionQuery>(pagedDatabaseCommandProvider);
        var query = new Mock<IFieldSelectionQuery>().Object;

        // Act
        var actual = sut.TryCreate(query, out var provider);

        // Assert
        actual.Should().BeTrue();
        provider.Should().BeSameAs(pagedDatabaseCommandProvider);
    }

    [Fact]
    public void TryCreate_Returns_False_When_Query_Is_Not_Of_Correct_Type()
    {
        // Arrange
        var pagedDatabaseCommandProvider = new Mock<IPagedDatabaseCommandProvider<ISingleEntityQuery>>().Object;
        var sut = new DefaultPagedDatabaseCommandProviderProvider<IFieldSelectionQuery>(pagedDatabaseCommandProvider);
        var query = new Mock<IGroupingQuery>().Object;

        // Act
        var actual = sut.TryCreate(query, out var provider);

        // Assert
        actual.Should().BeFalse();
        provider.Should().BeNull();
    }
}
