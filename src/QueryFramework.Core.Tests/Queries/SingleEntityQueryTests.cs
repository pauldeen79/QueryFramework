namespace QueryFramework.Core.Tests.Queries;

public class SingleEntityQueryTests
{
    [Fact]
    public void Can_Construct_SingleEntityQuery_With_Default_Values()
    {
        // Act
        var sut = new SingleEntityQuery();

        // Assert
        sut.Conditions.Should().BeEmpty();
        sut.Limit.Should().BeNull();
        sut.Offset.Should().BeNull();
        sut.OrderByFields.Should().BeEmpty();
    }

    [Fact]
    public void Can_Construct_SingleEntityQuery_With_Custom_Values()
    {
        // Arrange
        var conditions = new[] { new QueryCondition(false, false, new QueryExpression("field", null), QueryOperator.Equal, "value", QueryCombination.And) };
        var orderByFields = new[] { new QuerySortOrder(new QueryExpression("field", null), QuerySortOrderDirection.Ascending) };
        var limit = 1;
        var offset = 2;

        // Act
        var sut = new SingleEntityQuery(limit, offset, conditions, orderByFields);

        // Assert
        sut.Conditions.Should().BeEquivalentTo(conditions);
        sut.Limit.Should().Be(limit);
        sut.Offset.Should().Be(offset);
        sut.OrderByFields.Should().BeEquivalentTo(orderByFields);
    }

    [Fact]
    public void Constructing_SingleEntityQuery_With_ValidationError_Leads_To_Exception()
    {
        // Arrange
        var action = new Action(() => _ = new FieldSelectionQuery(null, null, false, true, new[] { new QueryCondition(true, false, new QueryExpression("field", null), QueryOperator.Equal, null, QueryCombination.And) }, Enumerable.Empty<IQuerySortOrder>(), Enumerable.Empty<IQueryExpression>()));

        // Act
        action.Should().Throw<ValidationException>();
    }

    [Fact]
    public void Can_Compare_SingleEntityQuery_With_Equal_Values()
    {
        // Arrange
        var q1 = new SingleEntityQuery(5, 64, new[] { new QueryCondition(false, false, new QueryExpression("Field", null), QueryOperator.Equal, "A", QueryCombination.And) }, Enumerable.Empty<IQuerySortOrder>());
        var q2 = new SingleEntityQuery(5, 64, new[] { new QueryCondition(false, false, new QueryExpression("Field", null), QueryOperator.Equal, "A", QueryCombination.And) }, Enumerable.Empty<IQuerySortOrder>());

        // Act
        var actual = q1.Equals(q2);

        // Asset
        actual.Should().BeTrue();
    }
}
