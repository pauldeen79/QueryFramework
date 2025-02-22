namespace QueryFramework.Core.Tests.Queries.Builders;

public class FieldSelectionQueryBuilderTests
{
    [Fact]
    public void Can_Construct_FieldSelectionQueryBuilder_With_Default_Values()
    {
        // Act
        var sut = new FieldSelectionQueryBuilder();

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
    public void Can_Construct_FieldSelectionQueryBuilder_With_Custom_Values()
    {
        // Arrange
        var conditions = new[] { new ComposableEvaluatableBuilder().WithLeftExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("field"))
                                                                   .WithOperator(new EqualsOperatorBuilder())
                                                                   .WithRightExpression(new ConstantExpressionBuilder().WithValue("value")) };
        var orderByFields = new[] { new QuerySortOrderBuilder().WithFieldName("field") };
        var limit = 1;
        var offset = 2;
        var distinct = true;
        var getAllFields = true;
        var fields = new[] { "field" };

        // Act
        var sut = new FieldSelectionQueryBuilder
        {
            Filter = new ComposedEvaluatableBuilder().AddConditions(conditions),
            OrderByFields = orderByFields.ToList(),
            Limit = limit,
            Offset = offset,
            Distinct = distinct,
            GetAllFields = getAllFields,
            FieldNames = fields.ToList()
        };

        // Assert
        sut.Filter.Conditions.ToArray().ShouldBeEquivalentTo(conditions);
        sut.Distinct.ShouldBe(distinct);
        sut.FieldNames.ToArray().ShouldBeEquivalentTo(fields);
        sut.GetAllFields.ShouldBe(getAllFields);
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
        var distinct = true;
        var getAllFields = true;
        var fields = new[] { "field" };
        var sut = new FieldSelectionQueryBuilder
        {
            Filter = new ComposedEvaluatableBuilder().AddConditions(conditions),
            OrderByFields = orderByFields.ToList(),
            Limit = limit,
            Offset = offset,
            Distinct = distinct,
            GetAllFields = getAllFields,
            FieldNames = fields.ToList()
        };

        // Act
        var actual = sut.BuildTyped();

        // Assert
        actual.ShouldNotBeNull();
        actual.Distinct.ShouldBe(sut.Distinct);
        actual.GetAllFields.ShouldBe(sut.GetAllFields);
        actual.Limit.ShouldBe(sut.Limit);
        actual.Offset.ShouldBe(sut.Offset);
        actual.Filter.Conditions.Count.ShouldBe(sut.Filter.Conditions.Count);
        actual.FieldNames.Count.ShouldBe(sut.FieldNames.Count);
        actual.OrderByFields.Count.ShouldBe(sut.OrderByFields.Count);
    }
}
