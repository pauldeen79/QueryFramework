namespace QueryFramework.Core.Tests.Queries;

public class FieldSelectionQueryTests
{
    [Fact]
    public void Can_Construct_FieldSelectionQuery_With_Default_Values()
    {
        // Act
        var sut = new FieldSelectionQuery();

        // Assert
        sut.Filter.Conditions.Should().BeEmpty();
        sut.Distinct.Should().BeFalse();
        sut.FieldNames.Should().BeEmpty();
        sut.GetAllFields.Should().BeFalse();
        sut.Limit.Should().BeNull();
        sut.Offset.Should().BeNull();
        sut.OrderByFields.Should().BeEmpty();
    }

    [Fact]
    public void Can_Construct_FieldSelectionQuery_With_Custom_Values()
    {
        // Arrange
        var conditions = new[]
        {
            new ComposableEvaluatableBuilder()
                .WithLeftExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("field"))
                .WithOperator(new EqualsOperatorBuilder())
                .WithRightExpression(new ConstantExpressionBuilder().WithValue("value"))
        };
        var orderByFields = new[] { new QuerySortOrderBuilder().WithFieldName("field")
                                                               .WithOrder(QuerySortOrderDirection.Ascending)
                                                               .Build() };
        var limit = 1;
        var offset = 2;
        var distinct = true;
        var getAllFields = true;
        var fields = new[] { "field" };

        // Act
        var sut = new FieldSelectionQuery(limit, offset, distinct, getAllFields, new ComposedEvaluatableBuilder().AddConditions(conditions).BuildTyped(), orderByFields, fields);

        // Assert
        sut.Filter.Conditions.Should().BeEquivalentTo(conditions);
        sut.Distinct.Should().Be(distinct);
        sut.FieldNames.Should().BeEquivalentTo(fields);
        sut.GetAllFields.Should().Be(getAllFields);
        sut.Limit.Should().Be(limit);
        sut.Offset.Should().Be(offset);
        sut.OrderByFields.Should().BeEquivalentTo(orderByFields);
    }
}
