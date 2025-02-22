namespace QueryFramework.Core.Tests.Queries;

public class FieldSelectionQueryTests
{
    [Fact]
    public void Can_Construct_FieldSelectionQuery_With_Default_Values()
    {
        // Act
        var sut = new FieldSelectionQueryBuilder().BuildTyped();

        // Assert
        sut.Filter.Conditions.ShouldBeEmpty();
        sut.Distinct.ShouldBeFalse();
        sut.FieldNames.ShouldBeEmpty();
        sut.GetAllFields.ShouldBeFalse();
        sut.Limit.ShouldBeNull();
        sut.Offset.ShouldBeNull();
        sut.OrderByFields.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Construct_FieldSelectionQuery_With_Custom_Values_All_Fields()
    {
        // Arrange
        var conditions = new[]
        {
            new ComposableEvaluatableBuilder()
                .WithLeftExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("field"))
                .WithOperator(new EqualsOperatorBuilder())
                .WithRightExpression(new ConstantExpressionBuilder().WithValue("value"))
        };
        var orderByFields = new[]
        {
            new QuerySortOrderBuilder()
                .WithFieldName("field")
                .WithOrder(QuerySortOrderDirection.Ascending)
        };
        var limit = 1;
        var offset = 2;
        var distinct = true;
        var getAllFields = true;

        // Act
        var sut = new FieldSelectionQueryBuilder()
            .WithLimit(limit)
            .WithOffset(offset)
            .OrderBy(orderByFields)
            .WithDistinct(distinct)
            .WithGetAllFields(getAllFields)
            .Where(conditions);

        // Assert
        sut.Filter.Conditions.ToArray().ShouldBeEquivalentTo(conditions);
        sut.Distinct.ShouldBe(distinct);
        sut.FieldNames.ShouldBeEmpty();
        sut.GetAllFields.ShouldBe(getAllFields);
        sut.Limit.ShouldBe(limit);
        sut.Offset.ShouldBe(offset);
        sut.OrderByFields.ToArray().ShouldBeEquivalentTo(orderByFields);
    }

    [Fact]
    public void Can_Construct_FieldSelectionQuery_With_Custom_Values_Specific_Fields()
    {
        // Arrange
        var conditions = new[]
        {
            new ComposableEvaluatableBuilder()
                .WithLeftExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("field"))
                .WithOperator(new EqualsOperatorBuilder())
                .WithRightExpression(new ConstantExpressionBuilder().WithValue("value"))
        };
        var orderByFields = new[]
        {
            new QuerySortOrderBuilder()
                .WithFieldName("field")
                .WithOrder(QuerySortOrderDirection.Ascending)
        };
        var limit = 1;
        var offset = 2;
        var distinct = true;
        var fields = new[] { "field" };

        // Act
        var sut = new FieldSelectionQueryBuilder()
            .WithLimit(limit)
            .WithOffset(offset)
            .OrderBy(orderByFields)
            .WithDistinct(distinct)
            .Select(fields)
            .Where(conditions);

        // Assert
        sut.Filter.Conditions.ToArray().ShouldBeEquivalentTo(conditions);
        sut.Distinct.ShouldBe(distinct);
        sut.FieldNames.ToArray().ShouldBeEquivalentTo(fields);
        sut.GetAllFields.ShouldBeFalse();
        sut.Limit.ShouldBe(limit);
        sut.Offset.ShouldBe(offset);
        sut.OrderByFields.ToArray().ShouldBeEquivalentTo(orderByFields);
    }
}
