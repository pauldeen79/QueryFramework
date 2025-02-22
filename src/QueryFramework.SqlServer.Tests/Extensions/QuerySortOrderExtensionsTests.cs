namespace QueryFramework.SqlServer.Tests.Extensions;

public class QuerySortOrderExtensionsTests
{
    [Fact]
    public void ToSql_Throws_On_Invalid_QuerySortOrder()
    {
        // Arrange
        var querySortOrder = new QuerySortOrderBuilder().WithFieldName("Field")
                                                        .WithOrder((QuerySortOrderDirection)99)
                                                        .Build();

        // Act & Assert
        Action a = () => querySortOrder.ToSql();
        a.ShouldThrow<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(QuerySortOrderDirection.Ascending, "ASC")]
    [InlineData(QuerySortOrderDirection.Descending, "DESC")]
    public void ToSql_Converts_Valid_QuerySortOrder_Correctly(QuerySortOrderDirection input, string expectedOutput)
    {
        // Arrange
        var querySortOrder = new QuerySortOrderBuilder().WithFieldName("Field")
                                                        .WithOrder(input)
                                                        .Build();

        // Act
        var actual = querySortOrder.ToSql();

        // Assert
        actual.ShouldBe(expectedOutput);
    }
}
