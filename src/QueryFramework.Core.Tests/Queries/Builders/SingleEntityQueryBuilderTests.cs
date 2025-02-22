namespace QueryFramework.Core.Tests.Queries.Builders;

public class SingleEntityQueryBuilderTests
{
    [Fact]
    public void Can_Construct_SingleEntityQueryBuilder_With_Default_Values()
    {
        // Act
        var sut = new SingleEntityQueryBuilder();

        // Assert
        sut.Filter.Conditions.ShouldBeEmpty();
        sut.Limit.ShouldBeNull();
        sut.Offset.ShouldBeNull();
        sut.OrderByFields.ShouldBeEmpty();
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
        sut.Filter.Conditions.ToArray().ShouldBeEquivalentTo(conditions);
        sut.Limit.ShouldBe(limit);
        sut.Offset.ShouldBe(offset);
        sut.OrderByFields.ToArray().ShouldBeEquivalentTo(orderByFields);
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
        actual.ShouldNotBeNull();
        actual.Limit.ShouldBe(sut.Limit);
        actual.Offset.ShouldBe(sut.Offset);
        actual.Filter.Conditions.Count.ShouldBe(sut.Filter.Conditions.Count);
        actual.OrderByFields.Count.ShouldBe(sut.OrderByFields.Count);
    }
}
