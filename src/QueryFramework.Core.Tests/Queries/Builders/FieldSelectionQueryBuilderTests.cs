﻿namespace QueryFramework.Core.Tests.Queries.Builders;

public class FieldSelectionQueryBuilderTests
{
    [Fact]
    public void Can_Construct_FieldSelectionQueryBuilder_With_Default_Values()
    {
        // Act
        var sut = new FieldSelectionQueryBuilder();

        // Assert
        sut.Filter.Conditions.Should().BeEmpty();
        sut.Distinct.Should().BeFalse();
        sut.Fields.Should().BeEmpty();
        sut.GetAllFields.Should().BeFalse();
        sut.Limit.Should().BeNull();
        sut.Offset.Should().BeNull();
        sut.OrderByFields.Should().BeEmpty();
    }

    [Fact]
    public void Can_Construct_FieldSelectionQueryBuilder_With_Custom_Values()
    {
        // Arrange
        var conditions = new[] { new ComposableEvaluatableBuilder().WithLeftExpression(new FieldExpressionBuilder().WithFieldName("field"))
                                                                   .WithOperator(new EqualsOperatorBuilder())
                                                                   .WithRightExpression(new ConstantExpressionBuilder().WithValue("value")) };
        var orderByFields = new[] { new QuerySortOrderBuilder().WithField(new FieldExpressionBuilder().WithFieldName("field")) };
        var limit = 1;
        var offset = 2;
        var distinct = true;
        var getAllFields = true;
        var fields = new[] { new FieldExpressionBuilder().WithFieldName("field") };

        // Act
        var sut = new FieldSelectionQueryBuilder
        {
            Filter = new ComposedEvaluatableBuilder().AddConditions(conditions),
            OrderByFields = orderByFields.Cast<IQuerySortOrderBuilder>().ToList(),
            Limit = limit,
            Offset = offset,
            Distinct = distinct,
            GetAllFields = getAllFields,
            Fields = fields.Cast<ExpressionBuilder>().ToList()
        };

        // Assert
        sut.Filter.Conditions.Should().BeEquivalentTo(conditions);
        sut.Distinct.Should().Be(distinct);
        sut.Fields.Should().BeEquivalentTo(fields);
        sut.GetAllFields.Should().Be(getAllFields);
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
        var orderByFields = new[] { new QuerySortOrderBuilder().WithField(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("field")) };
        var limit = 1;
        var offset = 2;
        var distinct = true;
        var getAllFields = true;
        var fields = new[] { new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("field") };
        var sut = new FieldSelectionQueryBuilder
        {
            Filter = new ComposedEvaluatableBuilder().AddConditions(conditions),
            OrderByFields = orderByFields.Cast<IQuerySortOrderBuilder>().ToList(),
            Limit = limit,
            Offset = offset,
            Distinct = distinct,
            GetAllFields = getAllFields,
            Fields = fields.Cast<ExpressionBuilder>().ToList()
        };

        // Act
        var actual = sut.Build();

        // Assert
        actual.Should().NotBeNull();
        actual.Distinct.Should().Be(sut.Distinct);
        actual.GetAllFields.Should().Be(sut.GetAllFields);
        actual.Limit.Should().Be(sut.Limit);
        actual.Offset.Should().Be(sut.Offset);
        actual.Filter.Conditions.Should().HaveCount(sut.Filter.Conditions.Count());
        actual.Fields.Should().HaveCount(sut.Fields.Count);
        actual.OrderByFields.Should().HaveCount(sut.OrderByFields.Count);
    }
}
