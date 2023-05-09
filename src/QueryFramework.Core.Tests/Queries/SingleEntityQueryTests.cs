namespace QueryFramework.Core.Tests.Queries;

public class SingleEntityQueryTests
{
    [Fact]
    public void Can_Construct_SingleEntityQuery_With_Default_Values()
    {
        // Act
        var sut = new SingleEntityQuery();

        // Assert
        sut.Filter.Conditions.Should().BeEmpty();
        sut.Limit.Should().BeNull();
        sut.Offset.Should().BeNull();
        sut.OrderByFields.Should().BeEmpty();
    }

    [Fact]
    public void Can_Construct_SingleEntityQuery_With_Custom_Values()
    {
        // Arrange
        var conditions = new[]
        {
            new ComposableEvaluatableBuilder()
                .WithLeftExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldNameExpression("field"))
                .WithOperator(new EqualsOperatorBuilder())
                .WithRightExpression(new ConstantExpressionBuilder().WithValue("value"))
        };
        var orderByFields = new[] { new QuerySortOrderBuilder().WithFieldName("field")
                                                               .WithOrder(QuerySortOrderDirection.Ascending)
                                                               .Build() };
        var limit = 1;
        var offset = 2;

        // Act
        var sut = new SingleEntityQuery(limit, offset, new ComposedEvaluatableBuilder().AddConditions(conditions).BuildTyped(), orderByFields);

        // Assert
        sut.Filter.Conditions.Should().BeEquivalentTo(conditions);
        sut.Limit.Should().Be(limit);
        sut.Offset.Should().Be(offset);
        sut.OrderByFields.Should().BeEquivalentTo(orderByFields);
    }

    [Fact]
    public void Can_Compare_SingleEntityQuery_With_Equal_Values()
    {
        // Arrange
        var q1 = new SingleEntityQuery(5, 64, new ComposedEvaluatableBuilder()
            .AddConditions(new ComposableEvaluatableBuilder()
                .WithLeftExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldNameExpression("Field"))
                .WithOperator(new EqualsOperatorBuilder())
                .WithRightExpression(new ConstantExpressionBuilder().WithValue("A")))
            .BuildTyped(), Enumerable.Empty<IQuerySortOrder>());
        var q2 = new SingleEntityQuery(5, 64, new ComposedEvaluatableBuilder()
            .AddConditions(new ComposableEvaluatableBuilder()
                                                        .WithLeftExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldNameExpression("Field"))
                                                        .WithOperator(new EqualsOperatorBuilder())
                                                        .WithRightExpression(new ConstantExpressionBuilder().WithValue("A")))
            .BuildTyped(), Enumerable.Empty<IQuerySortOrder>());

        // Act
        var actual = q1.Equals(q2);

        // Asset
        actual.Should().BeTrue();
    }
}
