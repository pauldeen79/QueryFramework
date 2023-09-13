namespace QueryFramework.Core.Tests.Queries.Builders;

public class GroupingQueryBuilderTests
{
    [Fact]
    public void Can_Construct_GroupingQueryBuilder_With_Default_Values()
    {
        // Act
        var sut = new GroupingQueryBuilder();

        // Assert
        sut.Filter.Conditions.Should().BeEmpty();
        sut.Limit.Should().BeNull();
        sut.Offset.Should().BeNull();
        sut.OrderByFields.Should().BeEmpty();
        sut.GroupByFields.Should().BeEmpty();
        sut.GroupByFilter.Should().NotBeNull();
    }

    [Fact]
    public void Can_Construct_GroupingQueryBuilder_With_Custom_Values()
    {
        // Arrange
        var conditions = new[] { new ComposableEvaluatableBuilder().WithLeftExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("field"))
                                                                   .WithOperator(new EqualsOperatorBuilder())
                                                                   .WithRightExpression(new ConstantExpressionBuilder().WithValue("value")) };
        var orderByFields = new[] { new QuerySortOrderBuilder().WithFieldName("field") };
        var groupByFields = new[] { new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("field") };
        var limit = 1;
        var offset = 2;

        // Act
        var sut = new GroupingQueryBuilder
        {
            Filter = new ComposedEvaluatableBuilder().AddConditions(conditions),
            OrderByFields = orderByFields.Cast<IQuerySortOrderBuilder>().ToList(),
            Limit = limit,
            Offset = offset,
            GroupByFields = groupByFields.Cast<ExpressionBuilder>().ToList(),
            GroupByFilter = new ComposedEvaluatableBuilder().AddConditions(new ComposableEvaluatableBuilder().WithLeftExpression(new EmptyExpressionBuilder()).WithOperator(new EqualsOperatorBuilder()).WithRightExpression(new EmptyExpressionBuilder())),
        };

        // Assert
        sut.Filter.Conditions.Should().BeEquivalentTo(conditions);
        sut.Limit.Should().Be(limit);
        sut.Offset.Should().Be(offset);
        sut.OrderByFields.Should().BeEquivalentTo(orderByFields);
        sut.GroupByFields.Should().HaveCount(1);
        sut.GroupByFilter.Conditions.Should().HaveCount(1);
    }

    [Fact]
    public void Can_Build_Entity_From_Builder()
    {
        // Arrange
        var conditions = new[] { new ComposableEvaluatableBuilder().WithLeftExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("field"))
                                                                   .WithOperator(new EqualsOperatorBuilder())
                                                                   .WithRightExpression(new ConstantExpressionBuilder().WithValue("value")) };
        var orderByFields = new[] { new QuerySortOrderBuilder().WithFieldName("field") };
        var groupByFields = new[] { new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("field") };
        var limit = 1;
        var offset = 2;
        var sut = new GroupingQueryBuilder
        {
            Filter = new ComposedEvaluatableBuilder().AddConditions(conditions),
            OrderByFields = orderByFields.Cast<IQuerySortOrderBuilder>().ToList(),
            Limit = limit,
            Offset = offset,
            GroupByFields = groupByFields.Cast<ExpressionBuilder>().ToList(),
            GroupByFilter = new ComposedEvaluatableBuilder().AddConditions(new ComposableEvaluatableBuilder().WithLeftExpression(new EmptyExpressionBuilder()).WithOperator(new EqualsOperatorBuilder()).WithRightExpression(new EmptyExpressionBuilder())),
        };

        // Act
        var actual = sut.BuildTyped();

        // Assert
        actual.Should().NotBeNull();
        actual.Limit.Should().Be(sut.Limit);
        actual.Offset.Should().Be(sut.Offset);
        actual.Filter.Conditions.Should().HaveCount(sut.Filter.Conditions.Count());
        actual.OrderByFields.Should().HaveCount(sut.OrderByFields.Count);
        actual.GroupByFields.Should().HaveCount(1);
        actual.GroupByFilter.Conditions.Should().HaveCount(1);
    }
}
