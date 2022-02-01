namespace QueryFramework.SqlServer.Tests.Extensions;

public class QuerySortOrderExtensionsTests
{
    [Fact]
    public void ToSql_Throws_On_Invalid_QuerySortOrder()
    {
        // Arrange
        var querySortOrder = new QuerySortOrder(new QueryExpression("Field", null), (QuerySortOrderDirection)99);

        // Act & Assert
        querySortOrder.Invoking(x => x.ToSql())
                      .Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(QuerySortOrderDirection.Ascending, "ASC")]
    [InlineData(QuerySortOrderDirection.Descending, "DESC")]
    public void ToSql_Converts_Valid_QuerySortOrder_Correctly(QuerySortOrderDirection input, string expectedOutput)
    {
        // Arrange
        var querySortOrder = new QuerySortOrder(new QueryExpression("Field", null), input);

        // Act
        var actual = querySortOrder.ToSql();

        // Assert
        actual.Should().Be(expectedOutput);
    }
}
