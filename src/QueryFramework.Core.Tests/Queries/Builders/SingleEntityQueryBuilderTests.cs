namespace QueryFramework.Core.Tests.Queries.Builders;

public class SingleEntityQueryBuilderTests
{
    [Fact]
    public void Can_Construct_SingleEntityQueryBuilder_With_Default_Values()
    {
        // Act
        var sut = new SingleEntityQueryBuilder();

        // Assert
        sut.Filter.Conditions.Should().BeEmpty();
        sut.Limit.Should().BeNull();
        sut.Offset.Should().BeNull();
        sut.OrderByFields.Should().BeEmpty();
    }

    [Fact]
    public void Can_Construct_SingleEntityQueryBuilder_With_Custom_Values()
    {
        // Arrange
        var conditions = new[] { new ComposableEvaluatableBuilder().WithLeftExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("field"))
                                                                   .WithOperator(new EqualsOperatorBuilder())
                                                                   .WithRightExpression(new ConstantExpressionBuilder().WithValue("value")) };
        var orderByFields = new[] { new QuerySortOrderBuilder().WithFieldName("field") };
        var limit = 1;
        var offset = 2;

        // Act
        var sut = new SingleEntityQueryBuilder
        {
            Filter = new ComposedEvaluatableBuilder().AddConditions(conditions),
            OrderByFields = orderByFields.ToList(),
            Limit = limit,
            Offset = offset
        };

        // Assert
        sut.Filter.Conditions.Should().BeEquivalentTo(conditions);
        sut.Limit.Should().Be(limit);
        sut.Offset.Should().Be(offset);
        sut.OrderByFields.Should().BeEquivalentTo(orderByFields);
    }

    [Fact]
    public void Can_Build_Entity_From_Builder()
    {
        // Arrange
        var conditions = new[] { new ComposableEvaluatableBuilder().WithLeftExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("field"))
                                                                   .WithOperator(new EqualsOperatorBuilder())
                                                                   .WithRightExpression(new ConstantExpressionBuilder().WithValue("value")) };
        var orderByFields = new[] { new QuerySortOrderBuilder().WithFieldName("field") };
        var limit = 1;
        var offset = 2;
        var sut = new SingleEntityQueryBuilder
        {
            Filter = new ComposedEvaluatableBuilder().AddConditions(conditions),
            OrderByFields = orderByFields.ToList(),
            Limit = limit,
            Offset = offset
        };

        // Act
        var actual = sut.Build();

        // Assert
        actual.Should().NotBeNull();
        actual.Limit.Should().Be(sut.Limit);
        actual.Offset.Should().Be(sut.Offset);
        actual.Filter.Conditions.Should().HaveCount(sut.Filter.Conditions.Count);
        actual.OrderByFields.Should().HaveCount(sut.OrderByFields.Count);
    }
}
