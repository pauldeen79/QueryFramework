namespace QueryFramework.Core.Tests.Queries.Builders;

public class GroupingQueryBuilderTests
{
    [Fact]
    public void Can_Construct_GroupingQueryBuilder_With_Default_Values()
    {
        // Act
        var sut = new GroupingQueryBuilder();

        // Assert
        sut.Filter.Conditions.ShouldBeEmpty();
        sut.Limit.ShouldBeNull();
        sut.Offset.ShouldBeNull();
        sut.OrderByFields.ShouldBeEmpty();
        sut.GroupByFields.ShouldBeEmpty();
        sut.GroupByFilter.ShouldNotBeNull();
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
            OrderByFields = orderByFields.ToList(),
            Limit = limit,
            Offset = offset,
            GroupByFields = groupByFields.Cast<ExpressionBuilder>().ToList(),
            GroupByFilter = new ComposedEvaluatableBuilder().AddConditions(new ComposableEvaluatableBuilder().WithLeftExpression(new EmptyExpressionBuilder()).WithOperator(new EqualsOperatorBuilder()).WithRightExpression(new EmptyExpressionBuilder())),
        };

        // Assert
        sut.Filter.Conditions.ToArray().ShouldBeEquivalentTo(conditions);
        sut.Limit.ShouldBe(limit);
        sut.Offset.ShouldBe(offset);
        sut.OrderByFields.ToArray().ShouldBeEquivalentTo(orderByFields);
        sut.GroupByFields.Count.ShouldBe(1);
        sut.GroupByFilter.Conditions.Count.ShouldBe(1);
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
            OrderByFields = orderByFields.ToList(),
            Limit = limit,
            Offset = offset,
            GroupByFields = groupByFields.Cast<ExpressionBuilder>().ToList(),
            GroupByFilter = new ComposedEvaluatableBuilder().AddConditions(new ComposableEvaluatableBuilder().WithLeftExpression(new EmptyExpressionBuilder()).WithOperator(new EqualsOperatorBuilder()).WithRightExpression(new EmptyExpressionBuilder())),
        };

        // Act
        var actual = sut.BuildTyped();

        // Assert
        actual.ShouldNotBeNull();
        actual.Limit.ShouldBe(sut.Limit);
        actual.Offset.ShouldBe(sut.Offset);
        actual.Filter.Conditions.Count.ShouldBe(sut.Filter.Conditions.Count);
        actual.OrderByFields.Count.ShouldBe(sut.OrderByFields.Count);
        actual.GroupByFields.Count.ShouldBe(1);
        actual.GroupByFilter.Conditions.Count.ShouldBe(1);
    }
}
